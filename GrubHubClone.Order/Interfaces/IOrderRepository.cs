using GrubHubClone.Common.Models;

namespace GrubHubClone.Order.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderModel> CreateAsync(OrderModel order);
        Task<List<OrderModel>> GetAllAsync();
    }
}