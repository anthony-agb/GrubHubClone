using GrubHubClone.Common.Enums;

namespace GrubHubClone.Common.Models;

public class OrderModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid RestaurantId { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderProductModel> Products { get; set; } = new();
    public OrderStatus Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
}
