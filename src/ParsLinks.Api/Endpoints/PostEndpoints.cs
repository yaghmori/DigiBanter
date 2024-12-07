using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

public static class PostEndpoints
{
    public static void MapBlogPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var post = routes.MapGroup(AppEndPoints.BlogPosts.Base);


        post.MapGet("/", GetAllPosts)
            .AllowAnonymous();
        post.MapGet("/{id}", GetPostById)
         .AllowAnonymous();
    }





    private static async Task<IResult> GetAllPosts(

        IBlogPostService postService,
        HttpContext context,
        CancellationToken cancellationToken,
        [FromQuery] string? lang = "en-US")
    {
        var result = await postService.GetPostAsync(context, cancellationToken, lang);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<List<BlogPostResponse>>.Success(result.Data));
    }

    private static async Task<IResult> GetPostById(
        [FromRoute] int id,
    IBlogPostService postService,
    HttpContext context,
    CancellationToken cancellationToken,
    [FromQuery] string? lang = "en-US")
    {
        var result = await postService.GetPostByIdAsync(id,context, cancellationToken, lang);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return Results.NotFound(result.ErrorMessage);
            else
                return Results.BadRequest(result.ErrorMessage);
        }
        return TypedResults.Ok(ApiResult<BlogPostResponse>.Success(result.Data));
    }

}
