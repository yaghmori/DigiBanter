using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Dto.Response;
using DigiBanter.Shared.ResultWrapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        HttpContext context,
        CancellationToken cancellationToken,
        [FromQuery] string? lang = "en-US")
    {
        var result = await podcastService.GetAllAsync(context, cancellationToken, lang);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<List<PodcastResponse>>.Success(result.Data));
    }

    private static async Task<IResult> GetById(
        [FromRoute] Guid id,
    IPodcastService podcastService,
    HttpContext context,
    CancellationToken cancellationToken,
    [FromQuery] string? lang = "en-US")
    {
        var result = await podcastService.GetByIdAsync(id, context, cancellationToken, lang);

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
