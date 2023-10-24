using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Common.ServerSentEvents;
using GrubHubClone.Payment.Interfaces;
using System.Runtime.Intrinsics.X86;

namespace GrubHubClone.Payment.Services;

public class PaymentService : IPaymentService
{
    private readonly IBusClient _busClient;
    private readonly IServerSentEventsService _sseService;

    public PaymentService(IBusClient busClient, IServerSentEventsService sseService)
    {
        _busClient = busClient;
        _sseService = sseService;
    }

    public async Task StartPaymentProcess(OrderCreatedMessage order)
    {
        string paymentUrl = $"https://paymemoney.pay/{order.Id}";

        await _busClient.PublishAsync<UpdateOrderStatusMessage>(new UpdateOrderStatusMessage
        {
            Id = order.Id,
            Status = "ProcessingPayment"
        });

        if (!_sseService.ClientIsConnected(order.Id.ToString())) 
        {
            _sseService.AddMessageToQueue(order.Id.ToString(), paymentUrl);
            return;
        }

        await _sseService.SendEventToClient<string>(order.Id.ToString(), paymentUrl);
    }

    public async Task PaymentConfirmed(string id)
    {
        var updatedOrder = new UpdateOrderStatusMessage
        {
            Id = Guid.Parse(id),
            Status = "PaymentConfirmed",
        };

        await _busClient.PublishAsync<UpdateOrderStatusMessage>(updatedOrder);

        if (!_sseService.ClientIsConnected(id.ToString())) 
        {
            _sseService.AddMessageToQueue(id.ToString(), updatedOrder.Status);
            return;
        }

        await _sseService.SendEventToClient<UpdateOrderStatusMessage>(id.ToString(), updatedOrder);

        //_sseService.RemoveClient(id.ToString());
    }
}
