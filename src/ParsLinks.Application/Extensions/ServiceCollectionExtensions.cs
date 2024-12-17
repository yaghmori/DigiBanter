using Microsoft.Extensions.DependencyInjection;
using ParsLinks.Application.ApiServices;

namespace ParsLinks.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {


        services.AddScoped<IPodcastApiService, PodcastApiService>();
        services.AddScoped<IBlogPostApiService, BlogPostApiService>();
        services.AddScoped<ILanguageApiService, LanguageApiService>();
        services.AddScoped<ICategoryApiService, CategoryApiService>();

        return services;


    }


}
