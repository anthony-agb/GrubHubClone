
using GrubHubClone.Common.Models;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IMenuRepository
    {
        Task<MenuModel> CreateAsync(MenuModel menu);
        Task<List<MenuModel>> GetAllAsync();
        Task<MenuModel> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(MenuModel menu);
    }
}