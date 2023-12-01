using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Dtos.MessageBus;

namespace GrubHubClone.Payment.Interfaces
{
    public interface IPaymentService
    {
        Task ConfirmPaymentAsync(Guid id);
        Task<PaymentDto> GetByIdAsync(Guid id);
        Task StartPaymentProcessAsync(OrderCreatedMessage order);
    }
}