using GrubHubClone.Restaurant.Models.Dtos;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IVenueService
    {
        Task<VenueDto> CreateAsync(VenueDto venue);
        Task<List<VenueDto>> GetAllAsync();
        Task<VenueDto> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(VenueDto venue);
    }
}