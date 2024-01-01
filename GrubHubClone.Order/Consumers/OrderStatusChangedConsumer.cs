using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Order.Interfaces;

namespace GrubHubClone.Order.Consumers;

public class OrderStatusChangedConsumer : ConsumerBase<OrderStatusChangedMessage>
{
    private readonly IOrderService _orderService;

    public OrderStatusChangedConsumer(IBusClient busClient, IServiceScopeFactory factory) : base(busClient)
    {
        _orderService = factory.CreateScope().ServiceProvider.GetRequiredService<IOrderService>();
    }

    protected override async Task ProcessMessage(OrderStatusChangedMessage message)
    {
        try
        {
            await _orderService.UpdateStatusAsync(new OrderDto 
            {
                Id = message.Id,
                Status = message.Status,
            });
        }
        catch (Exception)
        {
            throw;
        }
    }
}
