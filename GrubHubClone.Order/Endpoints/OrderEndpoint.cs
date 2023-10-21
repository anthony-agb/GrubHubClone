using GrubHubClone.Common.Dtos;
using GrubHubClone.Order.Interfaces;
using GrubHubClone.Order.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GrubHubClone.Order.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("api/order")
            .WithTags("Order");

        group.MapPost("", CreateOrder);

        group.MapGet("", GetOrders);
    }

    public static async Task<IResult> GetOrders(IOrderService vs)
    {
        try
        {
            var order = await vs.GetAllAsync();
            return TypedResults.Ok(order);
        }
        catch
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }

    public static async Task<IResult> CreateOrder(IOrderService vs,
        [FromBody] CreateOrderRequest order)
    {
        try
        {
            var newOrder = await vs.CreateAsync(new InvoiceDto
            {
                Name = order.Name,
                Description = order.Description,
            });

            return TypedResults.Ok(newOrder);
        }
        catch
        {
            return TypedResults.StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
