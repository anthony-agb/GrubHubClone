namespace GrubHubClone.Order.Models.Request;

public readonly record struct CreateOrderRequest(
    decimal TotalPrice);
