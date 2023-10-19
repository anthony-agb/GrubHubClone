using GrubHubClone.Restaurant.Exceptions;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Models.Domain;
using GrubHubClone.Restaurant.Models.Dtos;
using GrubHubClone.Restaurant.Models.Request.Menu;

namespace GrubHubClone.Restaurant.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _repository;
    private readonly ILogger _logger;

    public MenuService(IMenuRepository repository, ILogger<MenuService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<MenuDto> CreateAsync(MenuDto menu)
    {
        try
        {
            var newMenu = await _repository.CreateAsync(new Menu
            {
                Id = Guid.NewGuid(),
                Name = menu.Name,
                Description = menu.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            });

            return MapToDto(newMenu);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in MenuService.CreateAsync.");
            throw new ServiceException("Error creating menu.", ex);
        }
    }

    public async Task<MenuDto> GetByIdAsync(Guid id)
    {
        try
        {
            var menu = await _repository.GetByIdAsync(id);
            return MapToDto(menu);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in MenuService.GetByIdAsync.");
            throw new ServiceException("Error retrieving menu.", ex);
        }
    }

    public async Task<List<MenuDto>> GetAllAsync()
    {
        try
        {
            var menus = await _repository.GetAllAsync();
            return MapToDto(menus);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in MenuService.GetAllAsync.");
            throw new ServiceException("Error retrieving menus.", ex);
        }
    }

    public async Task UpdateAsync(MenuDto menu)
    {
        try
        {
            Menu oldData = await _repository.GetByIdAsync(menu.Id);

            await _repository.UpdateAsync(new Menu
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description,
                CreatedDate = oldData.CreatedDate,
                UpdatedDate = DateTime.Now,
            });
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in MenuService.UpdateAsync.");
            throw new ServiceException("Error updating menu.", ex);
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        try
        {
            await _repository.RemoveAsync(id);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in MenuService.RemoveAsync.");
            throw new ServiceException("Error removing menu.", ex); ;
        }
    }

    private MenuDto MapToDto(Menu menu)
    {
        return new MenuDto
        {
            Id = menu.Id,
            Name = menu.Name,
            Description = menu.Description,
        };
    }

    private List<MenuDto> MapToDto(List<Menu> menus)
    {
        List<MenuDto> menuDtos = new();

        foreach (var menu in menus)
        {
            menuDtos.Add(new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description,
            });
        }

        return menuDtos;
    }
}
