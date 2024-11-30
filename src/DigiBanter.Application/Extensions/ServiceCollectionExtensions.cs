using DigiBanter.Application.ApiServices;
using Microsoft.Extensions.DependencyInjection;

namespace DigiBanter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {


        services.AddScoped<IPodcastApiService, PodcastApiService>();
        services.AddScoped<IBlogPostApiService, BlogPostApiService>();
      
        return services;


    }


}
