using ParsLinks.Shared.Services.TimeZoneResolver;
using System.Security.Claims;

namespace ParsLinks.Api.Endpoints;

public static class ServiceCollectionExtensions
{

    public static void MapEndpoints(this WebApplication app)
    {

        app.MapBlogPostEndpoints();
        app.MapPodcastEndpoints();
        app.MapSeedEndpoints();
        app.MapAuthEndpoints();
        app.MapGet("/", () => "Hello world!");
        app.MapGet("/user/me", (ClaimsPrincipal claimprincipal) =>
        {
           return claimprincipal.Claims.ToDictionary(t => t.Type, v => v.Value);
        }).RequireAuthorization();


    }

}
