using DigiBanter.Application.ApiServices.PodcastApiService;
using Microsoft.Extensions.DependencyInjection;

namespace DigiBanter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {


        services.AddScoped<IPodcastApiService, PodcastApiService>();
      
        return services;


    }


}
