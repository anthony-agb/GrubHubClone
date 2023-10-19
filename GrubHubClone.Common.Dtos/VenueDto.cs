namespace GrubHubClone.Restaurant.Models.Dtos;

public readonly record struct VenueDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedDate,
    DateTime UpdatedDate
    );
