using GrubHubClone.Common.Dtos.MessageBus;

namespace GrubHubClone.Payment.Interfaces
{
    public interface IPaymentService
    {
        Task PaymentConfirmed(string id);
        Task StartPaymentProcess(OrderCreatedMessage order);
    }
}