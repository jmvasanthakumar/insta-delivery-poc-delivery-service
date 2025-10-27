using InstaDelivery.DeliveryService.Api.Constants;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Application.Services.Contracts;
using InstaDelivery.DeliveryService.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.DeliveryService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = AuthPolicy.BasicAccess)]
public class DeliveryAgentController(
    ILogger<DeliveryAgentController> logger,
    IDeliveryAgentService deliveryAgentService) : ControllerBase
{
    [HttpPost("registerAgent")]
    [Authorize(Policy = AuthPolicy.ElevatedAccess)]
    public async Task<IActionResult> CreateDeliveryAgent(CreateDeliveryAgentDto request)
    {
        try
        {
            var result = await deliveryAgentService.RegisterDeliveryAgentAsync(request, CancellationToken.None);
            return Ok(result);
        }
        catch (DeliveryAgentDetailsAlreadyExistException ex)
        {
            logger.LogError(ex, "A Delivery Agent with the provided details already exists");
            return Conflict(new { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating delivery agent");
            return StatusCode(500, new { ex.Message });
        }
    }

    [HttpPut("updateAgent")]
    [Authorize(Policy = AuthPolicy.ElevatedAccess)]
    public async Task<IActionResult> UpdateDeliveryAgent(DeliveryAgentDto request)
    {
        try
        {
            var result = await deliveryAgentService.UpdateDeliveryAgentAsync(request, CancellationToken.None);
            return Ok(result);
        }
        catch (DeliveryPartnerNotFoundException ex)
        {
            logger.LogError(ex, "Delivery Agent not found");
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating delivery agent");
            return StatusCode(500, new { ex.Message });
        }
    }

    [HttpPut("updateAgentStatus")]
    public async Task<IActionResult> UpdateDeliveryAgentStatus(DeliveryAgentStatusDto request)
    {
        try
        {
            await deliveryAgentService.UpdateDeliveryAgentStatusAsync(request, CancellationToken.None);
            return NoContent();
        }
        catch (DeliveryPartnerNotFoundException ex)
        {
            logger.LogError(ex, "Delivery Agent not found");
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating delivery agent");
            return StatusCode(500, new { ex.Message });
        }
    }
}