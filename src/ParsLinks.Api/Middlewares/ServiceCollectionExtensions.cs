using ParsLinks.Shared.Services.TimeZoneResolver;
using EAllyfe.Api.Middlewares;

namespace ParsLinks.Api.Middlewares;

public static class ServiceCollectionExtensions
{

    public static void UseMiddlewares(this WebApplication app)
    {

        app.UseMiddleware<LanguageMiddleware>();
        app.UseMiddleware<TimeZoneMiddleware>();
        app.UseExceptionHandler();
    }

}
