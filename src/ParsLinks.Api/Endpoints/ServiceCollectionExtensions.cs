using Microsoft.EntityFrameworkCore;
using ParsLinks.DataAccess.DbContext;
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
        app.MapLanguageEndpoints();
        app.MapCategoryEndpoints();
        app.MapGet("/", () => "Hello world!");
        app.MapGet("/user/me", (ClaimsPrincipal claimprincipal) =>
        {
            return claimprincipal.Claims.ToDictionary(t => t.Type, v => v.Value);
        }).RequireAuthorization();
        app.MapPost("/apply-migrations", async (AppDbContext context) =>
        {
            try
            {
                await context.Database.MigrateAsync();
                return Results.Ok(new { message = "Database migrations applied successfully." });
            }
            catch (Exception ex)
            {
                return Results.Problem("Error applying database migrations.", ex.Message);
            }
        });


    }

}
