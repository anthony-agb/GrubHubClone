using GrubHubClone.Common.Models;

namespace GrubHubClone.Payment.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PaymentModel> CreateAsync(PaymentModel payment);
        Task<List<PaymentModel>> GetAllAsync();
        Task<PaymentModel> GetByIdAsync(Guid id);
        Task<PaymentModel> GetByOrderIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(PaymentModel payment);
    }
}