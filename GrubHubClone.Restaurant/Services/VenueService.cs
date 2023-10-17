using GrubHubClone.Restaurant.Exceptions;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Models.Domain;
using GrubHubClone.Restaurant.Models.Dtos;
using GrubHubClone.Restaurant.Models.Request.Venue;

namespace GrubHubClone.Restaurant.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _repository;
    private readonly ILogger _logger;

    public VenueService(IVenueRepository repository, ILogger<VenueService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<VenueDto> CreateAsync(VenueDto venue)
    {
        try
        {
            var newVenue = await _repository.CreateAsync(new Venue
            {
                Id = Guid.NewGuid(),
                Name = venue.Name,
                Description = venue.Description,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            });

            return MapToDto(newVenue);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueService.CreateAsync.");
            throw new ServiceException("Error creating venue.", ex);
        }
    }

    public async Task<VenueDto> GetByIdAsync(Guid id)
    {
        try
        {
            var venue = await _repository.GetByIdAsync(id);
            return MapToDto(venue);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueService.GetByIdAsync.");
            throw new ServiceException("Error retrieving venue.", ex);
        }
    }

    public async Task<List<VenueDto>> GetAllAsync()
    {
        try
        {
            var venues = await _repository.GetAllAsync();
            return MapToDto(venues);
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueService.GetAllAsync.");
            throw new ServiceException("Error retrieving venues.", ex); throw;
        }
    }

    public async Task UpdateAsync(VenueDto venue)
    {
        try
        {
            Venue oldData = await _repository.GetByIdAsync(venue.Id);

            await _repository.UpdateAsync(new Venue
            {
                Id = venue.Id,
                Name = venue.Name,
                Description = venue.Description,
                CreatedDate = oldData.CreatedDate,
                UpdatedDate = DateTime.Now,
            });
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueService.UpdateAsync.");
            throw new ServiceException("Error updating venue.", ex);
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
            _logger.LogError(ex, "Error in VenueService.RemoveAsync.");
            throw new ServiceException("Error removing venue.", ex); ;
        }
    }

    private VenueDto MapToDto(Venue venue)
    {
        return new VenueDto
        {
            Id = venue.Id,
            Name = venue.Name,
            Description = venue.Description,
        };
    }

    private List<VenueDto> MapToDto(List<Venue> venues)
    {
        List<VenueDto> venueDtos = new();

        foreach (var venue in venues)
        {
            venueDtos.Add(new VenueDto
            {
                Id = venue.Id,
                Name = venue.Name,
                Description = venue.Description,
            });
        }

        return venueDtos;
    }
}
