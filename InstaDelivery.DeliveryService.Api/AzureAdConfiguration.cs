namespace InstaDelivery.DeliveryService.Api;

public class AzureAdConfiguration
{
    public string Instance { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}