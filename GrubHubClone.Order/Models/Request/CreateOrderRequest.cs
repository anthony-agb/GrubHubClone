namespace GrubHubClone.Order.Models.Request;

public readonly record struct CreateOrderRequest(
    Guid CustomerId,
    Guid RestaurantId,
    decimal TotalPrice,
    List<Guid> Products);
