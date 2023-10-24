using GrubHubClone.Common.Models;

namespace GrubHubClone.Common.Dtos;

public readonly record struct MenuDto(
    Guid Id,
    string Name,
    string Description,
    List<Product> Products
    );
