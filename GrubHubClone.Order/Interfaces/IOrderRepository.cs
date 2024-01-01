using GrubHubClone.Common.Models;

namespace GrubHubClone.Order.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderModel> CreateAsync(OrderModel order);
        Task<List<OrderModel>> GetAllAsync();
        Task<int> CountAsync(Guid id);
        Task UpdateStatusAsync(OrderModel order);
        Task UpdateAsync(OrderModel order);
        Task<OrderModel> GetByIdAsync(Guid id);
    }
}