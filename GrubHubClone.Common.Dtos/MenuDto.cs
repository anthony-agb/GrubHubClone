namespace GrubHubClone.Restaurant.Models.Dtos;

public readonly record struct MenuDto(
    Guid Id,
    string Name,
    string Description,
    List<Product> Products
    );
