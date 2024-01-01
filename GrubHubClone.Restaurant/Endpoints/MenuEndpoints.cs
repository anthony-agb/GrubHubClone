using GrubHubClone.Common.Dtos;
using GrubHubClone.Restaurant.Interfaces;
using GrubHubClone.Restaurant.Models.Request.Menu;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GrubHubClone.Restaurant.Endpoints;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/menu")
            .WithTags("Menu");

        group.MapPost("{venueId}", CreateMenu);

        group.MapGet("{venueId}", GetMenus);

        group.MapGet("{venueId}/{menuId}", GetMenu);

        group.MapPatch("{venueId}", UpdateMenu);

        group.MapDelete("{venueId}/{menuId}", DeleteMenu);
    }

    public static async Task<IResult> GetMenus(IMenuService vs)
    {
        try
        {
            var menu = await vs.GetAllAsync();
            return TypedResults.Ok(menu);
        }
        catch
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> GetMenu(IMenuService vs,
        Guid id)
    {
        try
        {
            var menu = await vs.GetByIdAsync(id);
            return TypedResults.Ok(menu);
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> CreateMenu(IMenuService vs,
        [FromBody] CreateMenuRequest menu)
    {
        try
        {
            var newMenu = await vs.CreateAsync(new MenuDto
            {
                Name = menu.Name,
                Description = menu.Description,
            });

            return TypedResults.Ok(newMenu);
        }
        catch
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> UpdateMenu(IMenuService vs,
        UpdateMenuRequest menu)
    {
        try
        {
            await vs.UpdateAsync(new MenuDto
            {
                Id = menu.Id,
                Name = menu.Name,
                Description = menu.Description,
            });

            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> DeleteMenu(IMenuService vs, Guid id)
    {
        try
        {
            await vs.RemoveAsync(id);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            if (ex.GetBaseException().Message.Contains("does not exist"))
                return TypedResults.BadRequest(ex.GetBaseException().Message);

            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
