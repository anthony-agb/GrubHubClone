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

    public async Task<OrderModel> CreateAsync(OrderModel order)
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
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.CreateAsync.");
            throw new DataAccessException("Error creating order.", ex);
        }
    }

    public async Task<List<OrderModel>> GetAllAsync()
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

    public async Task<int> CountAsync(Guid id) 
    {
        var count = await _db.Orders.CountAsync(x => x.Id == id);
        return count;
    }

    public async Task UpdateAsync(OrderModel order) 
    {
        try
        {
            var dbOrder = await _db.Orders
                .Where(v => v.Id == order.Id)
                .FirstOrDefaultAsync();

            if (dbOrder == null)
            {
                throw new DataAccessException($"Order with ID: '{order.Id}' does not exist.");
            }

            _db.Entry(dbOrder).State = EntityState.Detached;
            _db.Orders.Attach(order);
            _db.Entry(order).State = EntityState.Modified;
            _db.Orders.Update(order);

            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to update order with ID: '{order.Id}'.");
            }
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.UpdateAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.UpdateAsync.");
            throw new DataAccessException("Error updating order.", ex);
        }
    }

    public async Task UpdateStatusAsync(OrderModel order)
    {
        try
        {
            var dbOrder = await _db.Orders
                .Where(v => v.Id == order.Id)
                .FirstOrDefaultAsync();

            if (dbOrder == null)
            {
                throw new DataAccessException($"Order with ID: '{order.Id}' does not exist.");
            }

            dbOrder.UpdatedTime = DateTime.UtcNow;
            dbOrder.Status = order.Status;

            var entity = _db.Orders.Update(dbOrder);

            entity.Property(o => o.CreatedTime).IsModified = false;
            entity.Property(o => o.TotalPrice).IsModified = false;

            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to update order with ID: '{order.Id}'.");
            }
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.UpdateAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.UpdateAsync.");
            throw new DataAccessException("Error updating order.", ex);
        }
    }

    public async Task<OrderModel> GetByIdAsync(Guid id)
    {
        try
        {
            var order = await _db.Orders
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new DataAccessException($"Order with ID: '{id}' does not exist.");
            }

            return order;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.GetByIdAsync.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OrderRepository.GetByIdAsync.");
            throw new DataAccessException("Error retrieving order.", ex);
        }
    }
}
