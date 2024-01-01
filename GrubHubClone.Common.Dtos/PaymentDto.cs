using GrubHubClone.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Dtos;

public readonly record struct PaymentDto(
    Guid Id,
    Guid OrderId,
    PaymentStatus Status,
    DateTime CreatedTime,
    DateTime UpdatedTime,
    DateTime ExpirationTime);

