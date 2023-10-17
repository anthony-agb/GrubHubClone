using GrubHubClone.Restaurant.Models.Domain;
using GrubHubClone.Restaurant.Models.Dtos;
using GrubHubClone.Restaurant.Models.Request.Venue;

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