using GrubHubClone.Common.Dtos;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IMenuService
    {
        Task<MenuDto> CreateAsync(MenuDto menu);
        Task<List<MenuDto>> GetAllAsync();
        Task<MenuDto> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(MenuDto menu);
    }
}