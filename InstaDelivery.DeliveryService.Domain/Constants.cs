using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDelivery.DeliveryService.Domain
{
    public static class DeliveryStatus
    {
        public const string Pending = "Pending";
        public const string Assigned = "Assigned";
        public const string InTransit = "InTransit";
        public const string Delivered = "Delivered";
        public const string Cancelled = "Cancelled";
    }

    public static class Constants
    {
    }
}
