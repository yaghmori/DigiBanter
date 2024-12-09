using AutoMapper;
using ParsLinks.Api.ActionFilters;
using ParsLinks.Api.DbContextInterceptors;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.Services.TimeZoneResolver;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using EAllyfe.Api.Middlewares;
using ParsLinks.Shared.Constatns;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ParsLinks.Shared.Extensions;
using System.Security.Claims;
using EAllyfe.Api.Helpers;
using ParsLinks.Api.AuthorizationHandler;
using Microsoft.AspNetCore.Hosting;
using static ParsLinks.Shared.MappingProfile;

namespace ParsLinks.Api.Extensions;

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
        DotNetEnv.Env.Load();

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
        //builder.Services.AddAutoMapper(typeof(SomeProfile).Assembly);
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
    public static void ConfigureAuthentication(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration.Get<AppConfig>()!;

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.MetadataAddress = config.AuthOptions.MetadataAddress;
            options.Audience = config.AuthOptions.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = config.AuthOptions.ValidIssuer
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var path = context.HttpContext.Request.Path;
                    if (path.StartsWithSegments("/hubs"))
                    {
                        var token = context.Request.Query["access_token"];
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                        }
                        context.Token = token;
                        return Task.CompletedTask;
                    }
                    var accessToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }
                    else
                    {
                        context.Request.Cookies.TryGetValue(AppConstants.AccessToken, out var access);
                        if (!string.IsNullOrEmpty(access))
                        {
                            context.Token = access;
                        }
                    }
                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

                    var sessionId = context.Principal?.GetSessionId();
                    var userId = context.Principal?.GetUserId();

                    if (string.IsNullOrWhiteSpace(sessionId) || string.IsNullOrWhiteSpace(userId))
                    {
                        context.Fail("Required claims are missing in the token.");
                        return;
                    }


                    IEnumerable<Claim> claims;
                    try
                    {
                        claims = await JwtHelper.GetClaimsAsync(Guid.Parse(sessionId), Guid.Parse(userId), dbContext);
                    }
                    catch (Exception ex)
                    {
                        context.Fail($"Failed to retrieve claims: {ex.Message}");
                        return;
                    }

                    var tenantIdentity = new ClaimsIdentity("Identity");
                    tenantIdentity.AddClaims(claims);

                    context.Principal!.AddIdentity(tenantIdentity);
                }
            };
        });
    }

    public static void ConfigureAuthorization(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, ClaimRequirementHandler>();

        //Register app claims
        builder.Services.AddAuthorizationCore(options =>
        {
            var claimList = typeof(AppClaims).GetNestedTypes()
                .SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
            foreach (var claim in claimList)
            {
                var value = claim.GetValue(null);

                if (value != null)
                {
                    options.AddPolicy(value.ToString()!, policy => policy.RequireAuthenticatedUser()
                        .Requirements.Add(new ClaimRequirement(value.ToString()!)));
                }
            }
        });

    }


}
