using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Application.Services.Contracts;
using InstaDelivery.DeliveryService.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.DeliveryService.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public class OrderController(
    ILogger<OrderController> logger,
    IOrderService orderService,
    IDeliveryService deliveryService) : ControllerBase
{
    [HttpGet("availableOrders")]
    public async Task<IActionResult> GetAvailableOrders(CancellationToken ct)
    {
        try
        {
            var orders = await orderService.GetAvailableOrderAsync(ct);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching available orders");
            return StatusCode(500, new { ex.Message });

        }
    }

    [HttpPost("assignOrder")]
    public async Task<IActionResult> AssignOrderAsync(AssignOrderDto assignOrderDto, CancellationToken ct)
    {
        try
        {
            await deliveryService.AssignOrderAsync(assignOrderDto, ct);
            return NoContent();
        }
        catch (DeliveryRecordNotFoundException ex)
        {
            logger.LogError(ex, "Delivery record not found");
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while assigning the order");
            return StatusCode(500, new { ex.Message });
        }
    }


    [HttpPost("updateDeliveryStatus")]
    public async Task<IActionResult> UpdateDeliveryStatus(DeliveryStatusDto request, CancellationToken ct)
    {
        try
        {
            await deliveryService.UpdateDeliveryStatusAsync(request, ct);
            return NoContent();
        }
        catch (DeliveryRecordNotFoundException ex)
        {
            logger.LogError(ex, "Delivery record not found");
            return NotFound(new { ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating delivery status");
            return StatusCode(500, new { ex.Message });
        }

    }
}
