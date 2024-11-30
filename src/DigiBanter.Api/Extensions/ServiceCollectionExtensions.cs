using AutoMapper;
using DigiBanter.Api.ActionFilters;
using DigiBanter.Api.DbContextInterceptors;
using DigiBanter.DataAccess.DbContext;
using DigiBanter.Domain.Entities;
using DigiBanter.Shared;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using EAllyfe.Api.Middlewares;

namespace DigiBanter.Api.Extensions;

public static class ServiceCollectionExtensions
{

    public static void BindConfigurations(this WebApplicationBuilder builder)
    {
        BindAppConfiguration(builder);
        ConfigureControllerJsonSerializer(builder);
        ConfigureValidators(builder);
        ConfigureDbContexts(builder);
        ConfigureIdentity(builder);
        ConfigureMapper(builder);
        ConfigureExceptionHandler(builder);
    }

    static void BindAppConfiguration(WebApplicationBuilder builder)
    {

        DotNetEnv.Env.Load("/src/.env");
        var env = builder.Environment;
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        builder.Services.AddOptions<AppConfig>()
        .BindConfiguration(nameof(AppConfig));

        //var b= DotNetEnv.Env.Load(".env");

        var config = builder.Configuration.Get<AppConfig>()!;

        if (config == null)
        {
            throw new Exception("Application settings not found");
        }


    }
    static void ConfigureControllerJsonSerializer(WebApplicationBuilder builder)
    {

        var jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,

            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateParseHandling = DateParseHandling.DateTimeOffset,
            //Converters = { new IsoDateTimeConverter { DateTimeFormat = "o" } }

        };



        builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = jsonSettings.ReferenceLoopHandling;
            options.SerializerSettings.Formatting = jsonSettings.Formatting;
            options.SerializerSettings.NullValueHandling = jsonSettings.NullValueHandling;
            options.SerializerSettings.DateTimeZoneHandling = jsonSettings.DateTimeZoneHandling;
            options.SerializerSettings.DateFormatHandling = jsonSettings.DateFormatHandling;
            options.SerializerSettings.DateParseHandling = jsonSettings.DateParseHandling;
            options.SerializerSettings.Converters = jsonSettings.Converters;

        });

        builder.Services.AddSingleton(jsonSettings);
        builder.Services.AddScoped<ModelValidationAttribute>();

        builder.Services.AddMvc(config => config.RespectBrowserAcceptHeader = true);

        builder.Services.AddSingleton<IJsonSerializer>(sp =>
        {
            return new NewtonsoftJsonSerializer(jsonSettings);

            // return new SystemTextJsonSerializer(serializerOptions);
        });
    }
    static void ConfigureValidators(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining(typeof(AppConfig));
    }
    static void ConfigureDbContexts(WebApplicationBuilder builder)
    {
        var config = builder.Configuration.Get<AppConfig>()!;

        var environment = builder.Environment;
        builder.Services.AddScoped<SaveChangesInterceptor>();


        builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var interceptor = serviceProvider.GetRequiredService<SaveChangesInterceptor>();
            options.UseNpgsql(config.DatabaseOptions.ConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
            }).AddInterceptors(interceptor);

            if (environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
            }
        });


    }

    static void ConfigureIdentity(WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 1;
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.AllowedForNewUsers = false;
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        builder.Services.AddScoped<IPasswordHasher<AppDbContext>, PasswordHasher<AppDbContext>>();
    }
    static void ConfigureMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile(provider.GetService<ITimeZoneProvider>()!, provider.GetService<IJsonSerializer>()!));
        }).CreateMapper());
    }

    static void ConfigureExceptionHandler(WebApplicationBuilder builder)
    {
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
    }

}
