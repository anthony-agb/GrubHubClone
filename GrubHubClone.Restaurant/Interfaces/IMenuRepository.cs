
using GrubHubClone.Common.Models;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu> CreateAsync(Menu menu);
        Task<List<Menu>> GetAllAsync();
        Task<Menu> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Menu menu);
    }
}