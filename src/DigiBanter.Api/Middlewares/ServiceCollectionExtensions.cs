using DigiBanter.Shared.Services.TimeZoneResolver;
using EAllyfe.Api.Middlewares;

namespace DigiBanter.Api.Middlewares;

public static class ServiceCollectionExtensions
{

    public static void UseMiddlewares(this WebApplication app)
    {

        app.UseMiddleware<TimeZoneMiddleware>();
        app.UseExceptionHandler();
    }

}
