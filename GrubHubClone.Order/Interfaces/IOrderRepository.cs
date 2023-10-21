using GrubHubClone.Common.Models;

namespace GrubHubClone.Order.Interfaces
{
    public interface IOrderRepository
    {
        Task<Invoice> CreateAsync(Invoice order);
        Task<List<Invoice>> GetAllAsync();
    }
}