using GrubHubClone.Common.ServerSentEvents;
using GrubHubClone.Payment.Interfaces;
using System;
using System.Buffers;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace GrubHubClone.Payment.Endpoints;

public static class PaymentEndpoint
{
    public static void AddPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/payment")
            .WithTags("Payment");

        group.MapGet("sse", ListenForPaymentUrl);
        group.MapPost("Confirm", ConfirmPayment);
        group.MapPost("test", Test);
    }

    private static async Task Test(IServerSentEventsService sse)
    {
        await sse.SendEventToClient<string>("test", "Hello, this is a test");
        return;
    }

    private static async Task<IResult> ConfirmPayment(HttpContext context, IPaymentService paymentService)
    {
        if (!context.Request.Headers.ContainsKey("ClientId") && string.IsNullOrEmpty(context.Request.Headers["ClientId"]))
        {
            return TypedResults.BadRequest("Missing headers \"ClientId\".");
        }

        var clientId = context.Request.Headers["ClientId"];

        await paymentService.PaymentConfirmed(clientId.ToString());

        return TypedResults.Ok();
    }

    private static async Task ListenForPaymentUrl(HttpContext context, IServerSentEventsService sse)
    {
        try
        {
            var writer = sse.Init(context);

            if (writer.Exception != null) 
            {
                throw new HttpRequestException(writer.Exception.InnerException!.Message);
            }

            string clientId = context.Request.Headers["ClientId"]!;

            while (sse.ClientIsConnected(clientId))
            {
                await sse.SendQueuedMessagesToClient(clientId);
                var eventTask = sse.GetTaskCompletion(clientId);

                await eventTask.Task;
            }

            context.Response.OnCompleted(() => 
            {
                sse.CloseConnection(clientId);
                return Task.CompletedTask;
            });

            sse.RemoveClient(clientId);

            writer.Dispose();

            return;
        }
        catch(HttpRequestException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
        }
    }
}
