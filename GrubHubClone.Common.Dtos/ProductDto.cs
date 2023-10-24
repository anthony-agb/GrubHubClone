namespace GrubHubClone.Common.Dtos;

public readonly record struct ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price);
