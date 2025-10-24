using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;

namespace InstaDelivery.DeliveryService.Proxy
{
    internal abstract class ProxyBase
    {
        protected readonly HttpClient _http;
        protected readonly IConfiguration _config;
        protected readonly ITokenAcquisition _tokenAcquisition;
        protected readonly IConfidentialClientApplication _confidentialClient;

        protected ProxyBase(HttpClient http, IConfiguration configuration, ITokenAcquisition tokenAcquisitionClient)
        {   
            _config = configuration;
            _http = http;
            _tokenAcquisition = tokenAcquisitionClient;

            _confidentialClient = ConfidentialClientApplicationBuilder
           .Create(configuration["AzureAd:ClientId"])
           .WithClientSecret(configuration["AzureAd:ClientSecret"])
           .WithAuthority(new Uri($"{configuration["AzureAd:Instance"]}{configuration["AzureAd:TenantId"]}"))
           .Build();
        }

        protected async Task AuthorizeUserCredentialsAsync()
        {
            string[] scopes = new[] { $"api://{_config["ApiServices:OrderService:ClientId"]}/access_as_user" };
            var token = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}
