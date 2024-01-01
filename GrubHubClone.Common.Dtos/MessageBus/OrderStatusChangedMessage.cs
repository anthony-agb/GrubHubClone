using GrubHubClone.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Dtos.MessageBus;

public readonly record struct OrderStatusChangedMessage(
    Guid Id,
    OrderStatus Status
    );