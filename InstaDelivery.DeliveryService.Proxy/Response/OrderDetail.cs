using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDelivery.DeliveryService.Proxy.Response
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;

    }
}
