using GrubHubClone.Common.Enums;

namespace GrubHubClone.Common.Dtos;

public readonly record struct OrderDto(
    Guid Id,
    decimal TotalPrice,
    OrderStatus Status,
    DateTime CreatedTime,
    DateTime UpdatedTime);
