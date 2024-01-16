using GrubHubClone.Common.Enums;
using GrubHubClone.Common.Models;

namespace GrubHubClone.Common.Dtos;

public readonly record struct OrderDto(
    Guid Id,
    Guid CustomerId,
    Guid RestaurantId,
    decimal TotalPrice,
    List<Guid> Products,
    OrderStatus Status,
    DateTime CreatedTime,
    DateTime UpdatedTime);
