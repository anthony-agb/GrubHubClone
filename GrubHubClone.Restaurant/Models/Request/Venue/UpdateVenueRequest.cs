using System.ComponentModel.DataAnnotations;

namespace GrubHubClone.Restaurant.Models.Request.Venue;

public readonly record struct UpdateVenueRequest(
    [Required] Guid Id,
    [Required] string Name,
    [Required] string Description);
