using AutoMapper;
using DigiBanter.Application.Extensions;
using DigiBanter.Shared;
using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.Services;
using DigiBanter.Shared.Services.TimeZoneResolver;
using DigiBanter.Web.Client.HttpDelegateHnadlers;
using DigiBanter.Web.Client.Services;
using FluentValidation;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace DigiBanter.Web.Client.Extensions;

public static class ServiceCollectionExtensions
{

    public static void RegisterServices(this IServiceCollection services)
    {

        ConfigureControllerAndJsonSerializer(services);
        ConfigureAppSettings(services);
        ConfigureAutoMapper(services);
        ConfigureValidators(services);

        services.AddOptions();
        services.AddApiServices();
        services.AddScoped<ITimeZoneHelper, TimeZoneHelper>();
        services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();

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

        var _settings = configuration.Get<AppConfiguration>() ?? throw new Exception("Application settings not found");

        services.AddSingleton(_settings);

        AppConstants.ServerBaseAddress = _settings.Endpoints.Api;
        ConfigureHttpClients(services, _settings);
    }


    static void ConfigureHttpClients(IServiceCollection services, AppConfiguration configuration)
    {

        services.AddScoped<TimeZoneDelegateHandler>();


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

}
