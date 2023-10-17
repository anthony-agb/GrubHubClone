using GrubHubClone.Restaurant.Exceptions;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace GrubHubClone.Restaurant.DataAccess.Repositories;

public class VenueRepository : IVenueRepository
{
    private readonly DbContext _db;
    private readonly ILogger _logger;

    public VenueRepository(DbContext db, ILogger<VenueRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<Venue> CreateAsync(Venue venue)
    {
        try
        {
            var venueExists = await _db.Venues
                .Where(v => v.Id == venue.Id)
                .CountAsync();

            if (venueExists == 1)
            {
                throw new DataAccessException($"Venue with ID: '{venue.Id}' does not exist.");
            }

            await _db.Venues.AddAsync(venue);
            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException("Failed to create venue.");
            }

            return venue;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.CreateAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.CreateAsync.");
            throw new DataAccessException("Error creating venue.", ex);
        }
    }

    public async Task<List<Venue>> GetAllAsync()
    {
        try
        {
            var venues = await _db.Venues
                .ToListAsync();

            return venues;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.GetAllAsync.");
            throw new DataAccessException("Error retrieving venues.", ex);
        }
    }

    public async Task<Venue> GetByIdAsync(Guid id)
    {
        try
        {
            var venue = await _db.Venues
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (venue == null)
            {
                throw new DataAccessException($"Venue with ID: '{id}' does not exist.");
            }

            return venue;
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.GetByIdAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.GetByIdAsync.");
            throw new DataAccessException("Error retrieving venue.", ex);
        }
    }

    public async Task UpdateAsync(Venue venue)
    {
        try
        {
            var dbVenue = await _db.Venues
                .Where(v => v.Id == venue.Id)
                .FirstOrDefaultAsync();

            if (dbVenue == null)
            {
                throw new DataAccessException($"Venue with ID: '{venue.Id}' does not exist.");
            }

            _db.Entry(dbVenue).State = EntityState.Detached;
            _db.Venues.Attach(venue);
            _db.Entry(venue).State = EntityState.Modified;
            _db.Venues.Update(venue);

            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to remove Venue with ID: '{venue.Id}'.");
            }
        }
        catch (DataAccessException ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.UpdateAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in VenueRepository.UpdateAsync.");
            throw new DataAccessException("Error updating venue.", ex);
        }
    }

    public async Task RemoveAsync(Guid id)
    {
        try
        {
            var venue = await _db.Venues
                .Where(v => v.Id == id)
                .FirstOrDefaultAsync();

            if (venue == null)
            {
                throw new DataAccessException($"Venue with ID: '{id}' does not exist."); ;
            }

            _db.Venues.Remove(venue);
            int success = await _db.SaveChangesAsync();

            if (success == 0)
            {
                throw new DataAccessException($"Failed to remove Venue with ID: '{id}'.");
            }
        }
        catch (DataAccessException ex) 
        {
            _logger.LogError(ex, "Error in VenueRepository.RemoveAsync.");
            throw ex;
        }
        catch (Exception ex)
        {
            _logger.LogError (ex, "Error in VenueRepository.RemoveAsync.");
            throw new DataAccessException("Error removing venue.", ex);
        }
    }
}
