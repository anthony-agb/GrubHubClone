using GrubHubClone.Restaurant.Models.Domain;

namespace GrubHubClone.Restaurant.Interfaces
{
    public interface IVenueRepository
    {
        Task<Venue> CreateAsync(Venue venue);
        Task<List<Venue>> GetAllAsync();
        Task<Venue> GetByIdAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Venue venue);
    }
}