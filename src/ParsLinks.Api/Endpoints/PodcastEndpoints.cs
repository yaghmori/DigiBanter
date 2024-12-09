using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ParsLinks.Shared.Models;
using Microsoft.Identity.Client;

public static class PodcastEndpoints
{
    public static void MapPodcastEndpoints(this IEndpointRouteBuilder routes)
    {
        var post = routes.MapGroup(AppEndPoints.Podcast.Base);


        post.MapGet("/", GetAll)
            .AllowAnonymous();
        post.MapGet("/{id}", GetById)
         .AllowAnonymous();
    }





    private static async Task<IResult> GetAll(
        IPodcastService podcastService,
        IConfiguration config,
        HttpContext context,
        CancellationToken cancellationToken,
        [FromQuery] string? lang = "en-US")
    {
        var appConfig = config.Get<AppConfig>();
        var result = await podcastService.GetAllAsync(context, appConfig, cancellationToken, lang);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<List<PodcastResponse>>.Success(result.Data));
    }

    private static async Task<IResult> GetById(
        [FromRoute] Guid id,
                IConfiguration config,


    IPodcastService podcastService,
    HttpContext context,
    CancellationToken cancellationToken,
    [FromQuery] string? lang = "en-US")
    {
        var appConfig = config.Get<AppConfig>();

        var result = await podcastService.GetByIdAsync(id, context, appConfig, cancellationToken, lang);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return Results.NotFound(result.ErrorMessage);
            else
                return Results.BadRequest(result.ErrorMessage);
        }
        return TypedResults.Ok(ApiResult<PodcastResponse>.Success(result.Data));
    }

}
