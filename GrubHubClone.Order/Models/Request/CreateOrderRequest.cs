namespace GrubHubClone.Order.Models.Request;

public readonly record struct CreateOrderRequest(
    string Name,
    string Description,
    decimal TotalPrice);
