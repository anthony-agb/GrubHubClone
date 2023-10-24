using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Dtos.MessageBus;

public readonly record struct OrderCreatedMessage(
    Guid Id,
    string Name,
    string Description,
    decimal TotlalPrice,
    string Status
    );
