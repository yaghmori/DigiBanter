using AutoMapper;
using ParsLinks.Admin.Services;
using ParsLinks.Application.Extensions;
using ParsLinks.Shared;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.Services;
using ParsLinks.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http;
namespace ParsLinks.Admin.Extensions;

public static class ServiceCollectionExtensions
{

    public static void RegisterServices(this IServiceCollection services, string env)
    {

        ConfigureControllerAndJsonSerializer(services);
        ConfigureAppSettings(services, env);
        ConfigureAutoMapper(services);
        ConfigureValidators(services);

        services.AddOptions();
        services.AddApiServices();
        services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
        services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
        services.AddLocalization();
    }
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var config = serviceProvider.GetRequiredService<AppConfig>();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient(AppClientTypes.Auth);

        services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                var backendIdpUrl = config.OIDC.AuthServerUrl;// Environment.GetEnvironmentVariable("OIDC_IDP_ADDRESS_FOR_SERVER"); // "http://keycloak:8088/realms/lokalmaskin"
                var clientIdpUrl = "https://localhost:7011";// Environment.GetEnvironmentVariable("OIDC_IDP_ADDRESS_FOR_USERS"); // "http://localhost:8088/realms/lokalmaskin"

                options.Configuration = new()
                {
                    Issuer = backendIdpUrl,
                    AuthorizationEndpoint = config.AuthOptions.AuthenticationUrl,
                    TokenEndpoint = $"{backendIdpUrl}/protocol/openid-connect/token",
                    JwksUri = $"{backendIdpUrl}/protocol/openid-connect/certs",
                    JsonWebKeySet = FetchJwks($"{backendIdpUrl}/protocol/openid-connect/certs"),
                    EndSessionEndpoint = $"{clientIdpUrl}/protocol/openid-connect/logout",
                };
                Console.WriteLine("Jwks: " + options.Configuration.JsonWebKeySet);
                foreach (var key in options.Configuration.JsonWebKeySet.GetSigningKeys())
                {
                    options.Configuration.SigningKeys.Add(key);
                    Console.WriteLine("Added SigningKey: " + key.KeyId);
                }

                options.ClientId = Environment.GetEnvironmentVariable(config.OIDC.Resource); // "my_app"

                options.TokenValidationParameters.ValidIssuers = [clientIdpUrl, backendIdpUrl];
                options.TokenValidationParameters.NameClaimType = "name"; // This is what populates @context.User.Identity?.Name
                options.TokenValidationParameters.RoleClaimType = "role";
               //options.RequireHttpsMetadata = Environment.GetEnvironmentVariable("OIDC_REQUIRE_HTTPS_METADATA") != "false"; // disable only in dev env
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.MapInboundClaims = true;

                // options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        JsonWebKeySet FetchJwks(string url)
        {
            var result = httpClient.GetAsync(url).Result;
            if (!result.IsSuccessStatusCode || result.Content is null)
            {
                throw new Exception(
                    $"Getting token issuers (Keycloaks) JWKS from {url} failed. Status code {result.StatusCode}");
            }

            var jwks = result.Content.ReadAsStringAsync().Result;
            return new JsonWebKeySet(jwks);
        }
    }
    static void ConfigureControllerAndJsonSerializer(IServiceCollection services)
    {

        var jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "MM-dd-yyyy HH:mm:ss"
        };

        services.AddScoped(sp =>
        {
            return jsonSettings;
        });




        services.AddScoped<IJsonSerializer>(sp =>
        {
            return new NewtonsoftJsonSerializer(jsonSettings);
        });
    }


    static void ConfigureAppSettings(IServiceCollection services, string env)
    {

        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .Build();

        var _settings = configuration.Get<AppConfig>() ?? throw new Exception("Application settings not found");

        services.AddSingleton(_settings);

        AppConstants.ApiBaseAddress = _settings.Endpoints.Api;
        AppConstants.AuthBaseAddress = _settings.Endpoints.Auth;
        ConfigureHttpClients(services, _settings);
    }


    static void ConfigureHttpClients(IServiceCollection services, AppConfig configuration)
    {

        services.AddScoped<TimeZoneDelegateHandler>();


        services.AddHttpClient(AppClientTypes.Master, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Endpoints.Api);
        });
        //.AddHttpMessageHandler<TimeZoneDelegateHandler>();

        services.AddHttpClient(AppClientTypes.Auth, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Endpoints.Auth);
        });
            //.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();



    }

    static void ConfigureAutoMapper(IServiceCollection services)
    {
        services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile(provider.GetService<ITimeZoneProvider>(), provider.GetService<IJsonSerializer>()));
        }).CreateMapper());
    }

    static void ConfigureValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<MappingProfile>();
    }

}
