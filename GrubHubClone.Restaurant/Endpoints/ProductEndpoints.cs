namespace GrubHubClone.Restaurant.Endpoints;

public static class ProductEndpoints
{
    public static void MapRestaurantEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products");

        group.MapPost("{restaurantId}", CreateProducts);

        group.MapGet("{restaurantId}", GetProducts);

        group.MapGet("{restaurantId}", GetProduct);

        group.MapPatch("{restaurantId}", UpdateProduct);

        group.MapDelete("{restaurantId}", DeleteProduct);
    }

    public static Task<IResult> GetProducts() 
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> GetProduct()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> CreateProducts()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> UpdateProduct()
    {
        throw new NotImplementedException();
    }

    public static Task<IResult> DeleteProduct()
    {
        throw new NotImplementedException();
    }
}
