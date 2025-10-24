using InstaDelivery.DeliveryService.Application.Contracts;
using InstaDelivery.DeliveryService.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstaDelivery.DeliveryService.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public class OrderController(IOrderService orderService, IDeliveryService deliveryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAvailableOrders(CancellationToken ct)
    {
        var orders = await orderService.GetAvailableOrderAsync(ct);
        return Ok(orders);
    }

    [HttpPost]
    public async Task<IActionResult> AssignOrderAsync(AssignOrderDto assignOrderDto, CancellationToken ct)
    {
        var result = await deliveryService.AssignOrderAsync(assignOrderDto, ct);
        return Ok(result);
    }
}
