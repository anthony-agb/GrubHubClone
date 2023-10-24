namespace GrubHubClone.Common.Dtos;

public readonly record struct VenueDto(
    Guid Id,
    string Name,
    string Description,
    DateTime CreatedDate,
    DateTime UpdatedDate
    );
