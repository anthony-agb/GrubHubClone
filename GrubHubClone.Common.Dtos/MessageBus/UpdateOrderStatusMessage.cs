using GrubHubClone.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public readonly record struct UpdateOrderStatusMessage(
    Guid Id,
    OrderStatus Status
    );