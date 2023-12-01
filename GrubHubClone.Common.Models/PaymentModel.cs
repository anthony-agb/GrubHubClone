using GrubHubClone.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Models;

public class PaymentModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime UpdatedTime { get; set; }
    public DateTime ExpirationTime { get; set; }
}
