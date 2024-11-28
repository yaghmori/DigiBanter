using AutoMapper;
using DigiBanter.Api.ActionFilters;
using DigiBanter.DataAccess;
using DigiBanter.Domain.Entities;
using DigiBanter.Shared;
using DigiBanter.Shared.Models;
using DigiBanter.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using System.Xml;


AppConfig _config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
_config = builder.Configuration.Get<AppConfig>()!;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Map("/", () => "hello world!");
app.Run();

void BindAppConfiguration(WebApplicationBuilder builder)
{

    var env = builder.Environment.EnvironmentName;
    builder.Configuration
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.AddOptions<AppConfig>()
    .BindConfiguration(nameof(AppConfig));


    _config = builder.Configuration.Get<AppConfig>()!;

    if (_config == null)
    {
        throw new Exception("Application settings not found");
    }


}
void ConfigureControllerJsonSerializer(WebApplicationBuilder builder)
{

    var jsonSettings = new JsonSerializerSettings
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        Formatting = Newtonsoft.Json.Formatting.Indented,
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
void ConfigureValidators(WebApplicationBuilder builder)
{
    builder.Services.AddValidatorsFromAssemblyContaining(typeof(DigiBanter.Shared.Models.AppConfig));
}

void ConfigureDbContexts(WebApplicationBuilder builder)
{
    var environment = builder.Environment;

    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(_config.DatabaseOptions.ConnectionString, npgsqlOptions =>
        {
            npgsqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
        });


        // Enable sensitive data logging in development only
        if (environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
        }
    });

}

void ConfigureIdentity(WebApplicationBuilder builder)
{
    builder.Services.AddIdentity<User, DigiBanter.Domain.Entities.Role>(options =>
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
void ConfigureMapper(WebApplicationBuilder builder)
{
    builder.Services.AddScoped(provider => new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new MappingProfile(provider.GetService<ITimeZoneProvider>()!, provider.GetService<IJsonSerializer>()!));
    }).CreateMapper());
}