namespace GrubHubClone.Restaurant.Endpoints;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/menu");

        group.MapPost("{restaurantId}", CreateMenu);

        group.MapGet("{restaurantId}", GetMenus);

        group.MapGet("{restaurantId}/{menuId}", GetMenu);

        group.MapPatch("{restaurantId}/{menuId}", UpdateMenu);

        group.MapDelete("{restaurantId}/{menuId}", DeleteMenu);
    }

    public static Task<IResult> GetMenus() 
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> GetMenu()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> CreateMenu()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> UpdateMenu()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> DeleteMenu()
    {
        throw new NotImplementedException();
    }
}
