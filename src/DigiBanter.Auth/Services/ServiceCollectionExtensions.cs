using AutoMapper;
using DigiBanter.Shared;
using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.Services;
using DigiBanter.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Newtonsoft.Json;

namespace DigiBanter.Auth.Services;

public static class ServiceCollectionExtensions
{

    public static void RegisterAuthProjectServices(this IServiceCollection services)
    {

        ConfigureControllerAndJsonSerializer(services);
        ConfigureAppSettings(services);
        ConfigureAutoMapper(services);
        ConfigureValidators(services);
        ConfigureApiServices(services);
        services.AddOptions();

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


    static void ConfigureAppSettings(IServiceCollection services)
    {
        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var _settings = configuration.Get<EndPointConfiguration>() ?? throw new Exception("Application settings not found");

        services.AddSingleton(_settings);

        AppConstants.ApiBaseAddress = _settings.Endpoints.Api;
        ConfigureHttpClients(services, _settings);
    }


    static void ConfigureHttpClients(IServiceCollection services, EndPointConfiguration configuration)
    {

        services.AddHttpClient(AppClientTypes.Master, httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration.Endpoints.Api);
        });
        //.AddHttpMessageHandler<TimeZoneDelegateHandler>();

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
    static void ConfigureApiServices(IServiceCollection services)
    {
        services.AddScoped<AuthApiService>();
    }
}
