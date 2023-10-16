using Microsoft.AspNetCore.Mvc;

namespace GrubHubClone.Restaurant.Endpoints;

public static class RestaurantEndpoints
{
    public static void MapRestaurantEndpoints(this IEndpointRouteBuilder app) 
    {
        var group = app.MapGroup("api/restaurants");

        group.MapPost("", CreateRestaurant);

        group.MapGet("", GetRestaurants);

        group.MapGet("{id}", GetRestaurant);

        group.MapPatch("{id}", UpdateRestaurant);

        group.MapDelete("{id}", DeleteRestaurant);
    }

    public static async Task<IResult> GetRestaurants() 
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> GetRestaurant()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> CreateRestaurant()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> UpdateRestaurant()
    {
        throw new NotImplementedException();
    }

    public static async Task<IResult> DeleteRestaurant()
    {
        throw new NotImplementedException();
    }
}
