using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Payment.Interfaces;

namespace GrubHubClone.Payment.Consumers;

public class OrderCreatedConsumer : ConsumerBase<OrderCreatedMessage>
{
    private readonly IPaymentService _paymentService;

    public OrderCreatedConsumer(IBusClient busClient, IServiceScopeFactory factory) : base(busClient)
    {
        _paymentService = factory.CreateScope().ServiceProvider.GetRequiredService<IPaymentService>();
    }

    protected override async Task ProcessMessage(OrderCreatedMessage message)
    {
        await _paymentService.StartPaymentProcessAsync(message);
    }
}
