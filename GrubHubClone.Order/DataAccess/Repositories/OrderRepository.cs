using GrubHubClone.Common.Exceptions;
using GrubHubClone.Common.Models;
using GrubHubClone.Order.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Order.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _db;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(DatabaseContext db, ILogger<OrderRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Invoice> CreateAsync(Invoice order)
    {
        try
        {
            var orderExists = await _db.Orders
                .Where(o => o.Id == order.Id)
                .CountAsync();

            if (orderExists == 1)
            {
                throw new DataAccessException($"Order with ID: '{order.Id}' already exists.");
            }

            await _db.Orders.AddAsync(order);
            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException("Failed to create order.");
            }

            return order;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.CreateAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.CreateAsync.");
            throw new DataAccessException("Error creating order.", ex);
        }
    }

    public async Task<List<Invoice>> GetAllAsync()
    {
        try
        {
            var Orders = await _db.Orders
                .ToListAsync();

            return Orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.GetAllAsync.");
            throw new DataAccessException("Error retrieving orders.", ex);
        }
    }
}
