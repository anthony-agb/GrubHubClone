namespace GrubHubClone.Restaurant.Models.Request.Menu;

public readonly record struct CreateMenuRequest(
    string Name,
    string Description);
