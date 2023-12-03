using GrubHubClone.Common.Enums;

namespace GrubHubClone.Common.Models;

public class OrderModel
{
    public Guid Id { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
}
