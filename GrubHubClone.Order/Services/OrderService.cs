using GrubHubClone.Common.AzureServiceBus;
using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Dtos.MessageBus;
using GrubHubClone.Common.Enums;
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

    public async Task<OrderDto> CreateAsync(OrderDto order)
    {
        try
        {
            var newOrder = await _repository.CreateAsync(new OrderModel
            {
                Id = Guid.NewGuid(),
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                TotalPrice = order.TotalPrice,
                Products = MapToOrderProductModel(order),
                Status = OrderStatus.CREATED,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow,
            });

            await _client.PublishAsync<OrderCreatedMessage>(new OrderCreatedMessage
            {
                Id = newOrder.Id,
                TotlalPrice = newOrder.TotalPrice,
                Status = OrderStatus.CREATED
            });

            return MapToDto(newOrder);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderService.CreateAsync.");
            throw new ServiceException("Error creating order.", ex);
        }
    }

    public async Task<List<OrderDto>> GetAllAsync()
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


    public async Task<OrderDto> GetByIdAsync(Guid id)
    {
        try
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null) throw new Exception($"Order with ID: {id} does not exist.");

            return MapToDto(order);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateStatusAsync(OrderDto order) 
    {
        try
        {
            await _repository.UpdateStatusAsync(new OrderModel 
            {
                Id = order.Id,
                Status = order.Status
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task UpdateAsync(OrderDto order)
    {
        await _repository.UpdateAsync(new OrderModel
        {
            Id = Guid.NewGuid(),
            UpdatedTime = DateTime.Now,
        });
    }

    private OrderDto MapToDto(OrderModel order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            RestaurantId = order.RestaurantId,
            TotalPrice = order.TotalPrice,
            Products = order.Products
            .Select(x => x.ProductId)
            .ToList(),
            Status = order.Status,
            CreatedTime = order.CreatedTime,
            UpdatedTime = order.UpdatedTime
        };
    }

    private List<OrderDto> MapToDto(List<OrderModel> orders)
    {
        List<OrderDto> orderDtos = new();

        foreach (var order in orders)
        {
            orderDtos.Add(new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                RestaurantId = order.RestaurantId,
                TotalPrice = order.TotalPrice,
                Products = order.Products
                .Select(x => x.ProductId)
                .ToList(),
                Status = order.Status,
                CreatedTime = order.CreatedTime,
                UpdatedTime = order.UpdatedTime
            });
        }

        return orderDtos;
    }

    private List<OrderProductModel> MapToOrderProductModel(OrderDto order) 
    {
        List<OrderProductModel> orderProductModels = new();

        foreach (var product in order.Products) 
        {
            orderProductModels.Add(new OrderProductModel
            {
                OrderId = order.Id,
                ProductId = product
            });
        }

        return orderProductModels;
    }
}
