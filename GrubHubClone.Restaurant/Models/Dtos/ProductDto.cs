namespace GrubHubClone.Restaurant.Models.Dtos;

public readonly record struct ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price);
