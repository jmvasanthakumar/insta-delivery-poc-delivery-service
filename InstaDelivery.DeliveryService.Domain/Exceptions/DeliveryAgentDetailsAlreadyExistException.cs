using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaDelivery.DeliveryService.Domain.Exceptions
{
    public class DeliveryAgentDetailsAlreadyExistException : Exception
    {
        public DeliveryAgentDetailsAlreadyExistException(string phoneNumber, string email) 
            : base($"Delivery agent with Phone Number: {phoneNumber} and email {email} already exists")
        {

        }
    }
}
