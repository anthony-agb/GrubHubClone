using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrubHubClone.Common.Enums;

public enum OrderStatus
{
    UNDEFINED,
    CREATED,
    PROCESSING_PAYMENT,
    PAYED,
    ORDER_BEING_PREPARED,
    ORDER_READY_FOR_DELIVERY,
    ORDER_BEING_DELIVERD,
    DELIVERED,
    ORDER_READY_FOR_PICKUP,
    PICKEDUP
}
