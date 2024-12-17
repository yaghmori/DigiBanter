using Microsoft.AspNetCore.Mvc;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;
using System.Net;

public static class BlogPostEndpoints
{
    public static void MapBlogPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var posts = routes.MapGroup(AppEndPoints.BlogPosts.Base);


        posts.MapPost("/", AddPost).AllowAnonymous().DisableAntiforgery();
        posts.MapGet("/", GetAllPosts).AllowAnonymous();
        posts.MapGet("/{id}", GetPostById).AllowAnonymous();
    }


    private static async Task<IResult> AddPost(
        [FromForm] IFormCollection form,
        IBlogPostService postService,
        IJsonSerializer jsonSerializer,
        HttpContext context,
        CancellationToken cancellationToken)
    {

        var jsonData = form["request"].ToString();
        var request = jsonSerializer.Deserialize<BlogPostRequest>(jsonData);
        var image = form.Files.GetFile("image");

        var result = await postService.AddPostAsync(image, request, context, cancellationToken);

        if (!result.IsSuccess)
        {
            if (result.StatusCode == (int)HttpStatusCode.NotFound)
                return Results.NotFound(result.ErrorMessage);
            else
                return Results.BadRequest(result.ErrorMessage);
        }
        return TypedResults.Ok(ApiResult<int>.Success(result.Data));
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
        var result = await postService.GetPostByIdAsync(id, context, cancellationToken, lang);

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
