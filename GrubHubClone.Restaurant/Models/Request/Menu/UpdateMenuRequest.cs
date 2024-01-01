namespace GrubHubClone.Restaurant.Models.Request.Menu;

public readonly record struct UpdateMenuRequest(
    Guid Id,
    string Name,
    string Description);
