using InstaDelivery.DeliveryService.Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.DeliveryService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeliveryAgentController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateDeliveryAgent(CreateDeliveryAgentDto request)
    {
        // Implementation for registering a delivery agent goes here.
        return Ok("Delivery agent registered successfully.");
    }
}