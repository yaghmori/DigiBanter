﻿using AutoMapper;
using ParsLinks.Application.Extensions;
using ParsLinks.Shared;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.Services;
using ParsLinks.Shared.Services.TimeZoneResolver;
using ParsLinks.Web.Client.HttpDelegateHnadlers;
using ParsLinks.Web.Client.Services;
using FluentValidation;
using Newtonsoft.Json;
namespace ParsLinks.Web.Client.Extensions;

public static class ServiceCollectionExtensions
{

    public static void RegisterServices(this IServiceCollection services,string env)
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


    static void ConfigureAppSettings(IServiceCollection services,string env)
    {

        // Load configuration from appsettings.json
        var configuration = new ConfigurationBuilder()
            .AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: true)
            .Build();

        var _settings = configuration.Get<AppConfig>() ?? throw new Exception("Application settings not found");

        services.AddSingleton(_settings);

        AppConstants.ApiBaseAddress = _settings.Endpoints.Api;
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
