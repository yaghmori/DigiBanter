using ParsLinks.Shared.Services.TimeZoneResolver;

namespace ParsLinks.Api.Services;

public static class ServiceCollectionExtensions
{

    public static void RegisterServices(this WebApplicationBuilder builder)
    {

        builder.Services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();
        builder.Services.AddScoped<IBlogPostService, BlogPostService>();
        builder.Services.AddSingleton<IPodcastService, PodcastService>();
        builder.Services.AddSingleton<ILanguageReslover, LanguageResolver>();
        builder.Services.AddScoped<ILanguageService, LanguageService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();




    }

}
