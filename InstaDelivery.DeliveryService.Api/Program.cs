using FluentValidation;
using InstaDelivery.DeliveryService.Api.Configuration;
using InstaDelivery.DeliveryService.Api.Constants;
using InstaDelivery.DeliveryService.Api.Filters;
using InstaDelivery.DeliveryService.Api.HealthChecks;
using InstaDelivery.DeliveryService.Api.Validators;
using InstaDelivery.DeliveryService.Application;
using InstaDelivery.DeliveryService.Messaging;
using InstaDelivery.DeliveryService.Proxy;
using InstaDelivery.DeliveryService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.OpenApi.Models;
using Microsoft.ApplicationInsights;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
        });

        // Add services to the container.
        builder.Services.ConfigureRepositories(builder.Configuration);
        builder.Services.ConfigureProxyServices();

        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureMessagingServices();
        builder.Services.AddValidatorsFromAssemblyContaining<CreateDeliveryAgentDtoValidator>();

        builder.Services.AddControllers(opt =>
        {
            opt.Filters.Add<ValidationFilter>();
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var swaggerConfig = builder.Configuration.GetSection("SwaggerClient").Get<SwaggerConfiguration>()
           ?? throw new InvalidOperationException("Swagger configuration is missing or invalid.");

        builder.Services.AddSwaggerGen(c =>
        {
            var tenant = builder.Configuration["AzureAd:TenantId"];

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(string.Format(swaggerConfig.AuthorizeUrl, tenant)),
                        TokenUrl = new Uri(string.Format(swaggerConfig.TokenUrl, tenant)),
                        Scopes = new Dictionary<string, string>
                        {
                            {
                                $"{swaggerConfig.Scopes}", "Access API as the signed-in user"
                            }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } },
                    new[] { builder.Configuration["SwaggerClient:Scopes"] }
                }
            });
        });

        builder.Services.AddInMemoryTokenCaches();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi();

        builder.Services.AddAuthorizationBuilder()
            .AddPolicy(AuthPolicy.BasicAccess, policy =>
                  policy.RequireRole("User", "Admin"))
            .AddPolicy(AuthPolicy.ElevatedAccess, policy =>
                  policy.RequireRole("Admin"));

        builder.Services.ConfigureHealthCheck();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId(swaggerConfig.ClientId);
                c.OAuthUsePkce();
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.AddHealthChecks();

        app.Run();
    }
}