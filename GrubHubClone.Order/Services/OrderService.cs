using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Common.Exceptions;
using GrubHubClone.Common.Models;
using GrubHubClone.Order.Consumers;
using GrubHubClone.Order.Interfaces;

namespace GrubHubClone.Order.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly ILogger _logger;
    private readonly IBusClient _client;

    public OrderService(IOrderRepository repository, ILogger<OrderService> logger, IBusClient client)
    {
        _repository = repository;
        _logger = logger;
        _client = client;
    }

    public async Task<InvoiceDto> CreateAsync(InvoiceDto order)
    {
        try
        {
            var newOrder = await _repository.CreateAsync(new Invoice
            {
                Id = Guid.NewGuid(),
                Name = order.Name,
                Description = order.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            });

            await _client.PublishAsync<OrderCreatedMessage>(new OrderCreatedMessage
            {
                Id = newOrder.Id,
                Name = newOrder.Name,
                Description = newOrder.Description,
                TotlalPrice = newOrder.TotalPrice,
                Status = "PaymentPending"
            });

            return MapToDto(newOrder);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderService.CreateAsync.");
            throw new ServiceException("Error creating order.", ex);
        }
    }

    public async Task<List<InvoiceDto>> GetAllAsync()
    {
        try
        {
            var orders = await _repository.GetAllAsync();
            return MapToDto(orders);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderService.GetAllAsync.");
            throw new ServiceException("Error retrieving orders.", ex);
        }
    }

    private InvoiceDto MapToDto(Invoice order)
    {
        return new InvoiceDto
        {
            Id = order.Id,
            Name = order.Name,
            Description = order.Description,
            TotalPrice = order.TotalPrice,
        };
    }

    private List<InvoiceDto> MapToDto(List<Invoice> orders)
    {
        List<InvoiceDto> orderDtos = new();

        foreach (var order in orders)
        {
            orderDtos.Add(new InvoiceDto
            {
                Id = order.Id,
                Name = order.Name,
                Description = order.Description,
                TotalPrice = order.TotalPrice,
            });
        }

        return orderDtos;
    }
}
