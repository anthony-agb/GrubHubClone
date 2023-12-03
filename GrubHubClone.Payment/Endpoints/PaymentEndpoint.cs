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

        group.MapGet("details/{id}", PaymentDetails);
        group.MapPost("Confirm/{id}", ConfirmPayment);
    }

    private static async Task<IResult> PaymentDetails(HttpContext context, IPaymentService paymentService, string id)
    {
        if (id == null) 
        {
            return TypedResults.BadRequest("Missing payment ID.");
        }

        if (!Guid.TryParse(id, out Guid parsedId)) 
        {
            return TypedResults.BadRequest("Invalid payment ID.");
        }

        var payment = await paymentService.GetByOrderIdAsync(parsedId);

        return TypedResults.Ok(payment);
    }

    private static async Task<IResult> ConfirmPayment(HttpContext context, IPaymentService paymentService, string id)
    {
        if (id == null)
        {
            return TypedResults.BadRequest("Missing payment ID.");
        }

        if (!Guid.TryParse(id, out Guid parsedId))
        {
            return TypedResults.BadRequest("Invalid payment ID.");
        }

        await paymentService.ConfirmPaymentAsync(parsedId);

        return TypedResults.Ok();
    }
}
