using DigiBanter.Shared.Services.TimeZoneResolver;

namespace DigiBanter.Api.Endpoints;

public static class ServiceCollectionExtensions
{

    public static void MapEndpoints(this WebApplication app)
    {

        app.MapBlogPostEndpoints();
        app.MapPodcastEndpoints();
        app.MapSeedEndpoints();
        app.Map("/", () => Results.Redirect("/en"));

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

    }

}
