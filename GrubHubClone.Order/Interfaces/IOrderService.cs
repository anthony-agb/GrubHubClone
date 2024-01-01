using GrubHubClone.Common.Dtos;
using GrubHubClone.Common.Models;

namespace GrubHubClone.Order.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(OrderDto order);
        Task<List<OrderDto>> GetAllAsync();
        Task<OrderDto> GetByIdAsync(Guid id);
        Task UpdateStatusAsync(OrderDto order);
        Task UpdateAsync(OrderDto order);
    }
}