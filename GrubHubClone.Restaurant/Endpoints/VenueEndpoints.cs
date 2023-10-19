using GrubHubClone.Restaurant.Exceptions;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Models;
using GrubHubClone.Restaurant.Models.Dtos;
using GrubHubClone.Restaurant.Models.Request.Venue;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;

namespace GrubHubClone.Restaurant.Endpoints;

public static class VenueEndpoints
{
    public static void MapVenueEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/venues")
            .WithTags("Venue");

        group.MapPost("", CreateVenue);

        group.MapGet("", GetVenues);

        group.MapGet("{id}", GetVenue);

        group.MapPatch("", UpdateVenue);

        group.MapDelete("{id}", DeleteVenue);
    }

    public static async Task<IResult> GetVenues(IVenueService vs)
    {
        try
        {
            var venue = await vs.GetAllAsync();
            return TypedResults.Ok(venue);
        }
        catch
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> GetVenue(IVenueService vs,
        Guid id)
    {
        try
        {
            var venue = await vs.GetByIdAsync(id);
            return TypedResults.Ok(venue);
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> CreateVenue(IVenueService vs,
        [FromBody] CreateVenueRequest venue)
    {
        try
        {
            var newVenue = await vs.CreateAsync(new VenueDto 
            {
                Name = venue.Name,
                Description = venue.Description,
            });

            return TypedResults.Ok(newVenue);
        }
        catch 
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> UpdateVenue(IVenueService vs,
        UpdateVenueRequest venue)
    {
        try
        {
            await vs.UpdateAsync(new VenueDto 
            {
                Id = venue.Id,
                Name = venue.Name,
                Description = venue.Description,
            });

            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> DeleteVenue(IVenueService vs, Guid id)
    {
        try
        {
            await vs.RemoveAsync(id);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
