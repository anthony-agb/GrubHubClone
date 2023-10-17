using System.ComponentModel.DataAnnotations;

namespace GrubHubClone.Restaurant.Models.Request.Venue;

public readonly record struct CreateVenueRequest(
    [Required] string Name,
    [Required] string Description);
