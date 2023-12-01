
using GrubHubClone.Common.Models;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IVenueRepository
    {
        Task<VenueModel> CreateAsync(VenueModel venue);
        Task<List<VenueModel>> GetAllAsync();
        Task<VenueModel> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(VenueModel venue);
    }
}