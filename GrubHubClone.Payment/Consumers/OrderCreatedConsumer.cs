using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Payment.Interfaces;

namespace GrubHubClone.Payment.Consumers;

public class OrderCreatedConsumer : ConsumerBase<OrderCreatedMessage>
{
    private readonly IPaymentService _paymentService;

    public OrderCreatedConsumer(IBusClient busClient, IPaymentService paymentService) : base(busClient)
    {
        _paymentService = paymentService;
    }

    protected override async Task ProcessMessage(OrderCreatedMessage message)
    {
        await _paymentService.StartPaymentProcessAsync(message);
    }
}
