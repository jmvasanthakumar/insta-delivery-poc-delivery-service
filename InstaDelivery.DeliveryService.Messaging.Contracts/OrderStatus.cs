using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDelivery.DeliveryService.Messaging.Contracts
{
    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Assigned = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5,
    }
}
