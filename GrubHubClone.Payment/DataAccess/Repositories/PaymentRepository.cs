using GrubHubClone.Common.Exceptions;
using GrubHubClone.Common.Models;
using GrubHubClone.Payment.DataAccess;
using GrubHubClone.Payment.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrubHubClone.Payment.DataAccess.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly DatabaseContext _db;
    private readonly ILogger _logger;

    public PaymentRepository(DatabaseContext db, ILogger<PaymentRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<PaymentModel> CreateAsync(PaymentModel payment)
    {
        try
        {
            var paymentExists = await _db.Payments
                .Where(m => m.OrderId == payment.OrderId && m.ExpirationTime > DateTime.Now)
                .CountAsync();
            
            if (paymentExists == 1)
            {
                throw new DataAccessException($"Payment with ID: '{payment.Id}' already exists.");
            }

            await _db.Payments.AddAsync(payment);

            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException("Failed to create payment.");
            }

            return payment;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.CreateAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.CreateAsync.");
            throw new DataAccessException("Error creating payment.", ex);
        }
    }

    public async Task<List<PaymentModel>> GetAllAsync()
    {
        try
        {
            var payments = await _db.Payments
                .ToListAsync();

            return payments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.GetAllAsync.");
            throw new DataAccessException("Error retrieving payments.", ex);
        }
    }

    public async Task<PaymentModel> GetByIdAsync(Guid id)
    {
        try
        {
            var payment = await _db.Payments
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                throw new DataAccessException($"Payment with ID: '{id}' does not exist.");
            }

            return payment;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.GetByIdAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.GetByIdAsync.");
            throw new DataAccessException("Error retrieving payment.", ex);
        }
    }

    public async Task UpdateAsync(PaymentModel payment)
    {
        try
        {
            var dbPayment = await _db.Payments
                .Where(v => v.Id == payment.Id)
                .FirstOrDefaultAsync();

            if (dbPayment == null)
            {
                throw new DataAccessException($"Payment with ID: '{payment.Id}' does not exist.");
            }

            _db.Entry(dbPayment).State = EntityState.Detached;
            _db.Payments.Attach(payment);
            _db.Entry(payment).State = EntityState.Modified;
            _db.Payments.Update(payment);

            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to update Payment with ID: '{payment.Id}'.");
            }
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.UpdateAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.UpdateAsync.");
            throw new DataAccessException("Error updating payment.", ex);
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        try
        {
            var payment = await _db.Payments
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (payment == null)
            {
                throw new DataAccessException($"Payment with ID: '{id}' does not exist."); ;
            }

            _db.Payments.Remove(payment);
            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to remove payment with ID: '{id}'.");
            }
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.RemoveAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in PaymentRepository.RemoveAsync.");
            throw new DataAccessException("Error removing payment.", ex);
        }
    }
}
