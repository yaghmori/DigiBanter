using DigiBanter.Shared.Services.TimeZoneResolver;

namespace DigiBanter.Api.Services;

public static class ServiceCollectionExtensions
{

    public static void RegisterServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
        builder.Services.AddScoped<IBlogPostService, BlogPostService>();
        builder.Services.AddSingleton<IPodcastService, PodcastService>();
        builder.Services.AddSingleton<ILanguageService, LanguageService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

    }

}
