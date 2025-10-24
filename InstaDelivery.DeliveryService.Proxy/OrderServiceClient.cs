using InstaDelivery.DeliveryService.Proxy.Contracts;
using InstaDelivery.DeliveryService.Proxy.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace InstaDelivery.DeliveryService.Proxy;

internal class OrderServiceClient : ProxyBase, IOrderServiceClient
{
    public OrderServiceClient(HttpClient http, IConfiguration configuration, ITokenAcquisition tokenAcquisitionClient):base(http, configuration, tokenAcquisitionClient)
    {

    }

    /// <summary>
    /// Retrieves a list of available orders from the order service using OAuth2 client credentials flow.
    /// The method authenticates with Azure AD and makes a GET request to the order service endpoint.
    /// </summary>
    /// <param name="ct">Optional cancellation token to cancel the operation</param>
    /// <returns>A list of available orders. Returns an empty list if no orders are found.</returns>
    //public async Task<List<AvailableOrder>> GetAvailableOrdersAsync(CancellationToken ct = default)
    //{
    //    var scopes = new[] { $"{_config["ApiServices:OrderService:Scopes"]}" };
    //    var appToken = await _confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync(ct);

    //    _http.DefaultRequestHeaders.Authorization =
    //        new AuthenticationHeaderValue("Bearer", appToken.AccessToken);

    //    var response = await _http.GetAsync("orders/availableOrders", ct);
    //    response.EnsureSuccessStatusCode();
    //    var content = await response.Content.ReadFromJsonAsync<List<AvailableOrder>>(ct);
    //    return content ?? [];
    //}

    /// <summary>
    /// Retrieves a list of available orders from the order service using OAuth2 client credentials flow.
    /// The method authenticates with Azure AD and makes a GET request to the order service endpoint.
    /// </summary>
    /// <param name="ct">Optional cancellation token to cancel the operation</param>
    /// <returns>A list of available orders. Returns an empty list if no orders are found.</returns>
    public async Task<List<AvailableOrder>> GetAvailableOrdersAsync(CancellationToken ct = default)
    {
        await AuthorizeUserCredentialsAsync();

        var response = await _http.GetAsync("orders/availableOrders", ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<List<AvailableOrder>>(ct);
        return content ?? [];
    }

    public async Task<OrderDetail> GetOrderDetailsAsync(Guid orderId, CancellationToken ct = default)
    {
        await AuthorizeUserCredentialsAsync();

        var response = await _http.GetAsync($"orders/{orderId}", ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<OrderDetail>(ct);
        return content ?? new OrderDetail();
    }
}