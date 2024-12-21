using Microsoft.AspNetCore.Mvc;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.ResultWrapper;
using System.Net;

public static class BlogPostEndpoints
{
    public static void MapBlogPostEndpoints(this IEndpointRouteBuilder routes)
    {
        var posts = routes.MapGroup(AppEndPoints.BlogPosts.Base);


        posts.MapPost("/", AddPost).AllowAnonymous().DisableAntiforgery();
        posts.MapGet("/", GetAllPosts).AllowAnonymous();
        posts.MapGet("/{postId}", GetPostById).AllowAnonymous();
        posts.MapGet("/{postId}/detail", GetPostDetailById).AllowAnonymous();
        posts.MapDelete("/{postId}", DeletePostById).AllowAnonymous();
        posts.MapPatch("/", UpdatePostById).AllowAnonymous();
    }


    private static async Task<IResult> AddPost(
        [FromForm] IFormCollection form,
        IBlogPostService postService,
        IJsonSerializer jsonSerializer,
        CancellationToken cancellationToken)
    {

        var jsonData = form["request"].ToString();
        var request = jsonSerializer.Deserialize<BlogPostRequest>(jsonData);
        var image = form.Files.GetFile("image");

        var result = await postService.AddPostAsync(image, request, cancellationToken);

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
        CancellationToken cancellationToken,
        [AsParameters] BlogPostQueryParameters parameters)
    {
        return await postService.GetPostAsync(parameters, cancellationToken);

    }

    private static async Task<IResult> GetPostById(
        [FromRoute] int postId,
        [AsParameters] BlogPostQueryParameters parameters,
        IBlogPostService postService,
        CancellationToken cancellationToken)
    {
        return await postService.GetPostByIdAsync(postId, parameters, cancellationToken);
    }


    private static async Task<IResult> GetPostDetailById(
        [FromRoute] int postId,
        IBlogPostService postService,
        CancellationToken cancellationToken)
    {
        return await postService.GetPostDetailByIdAsync(postId, cancellationToken);
    }

    private static async Task<IResult> DeletePostById(
    [FromRoute] int postId,
    IBlogPostService postService,
    CancellationToken cancellationToken)
    {
        return await postService.DeletePostAsync(postId, cancellationToken);
    }

    private static async Task<IResult> UpdatePostById(
        [FromForm] IFormCollection form,
        IJsonSerializer jsonSerializer,
        IBlogPostService postService,
        CancellationToken cancellationToken)
    {
        var jsonData = form["request"].ToString();
        var request = jsonSerializer.Deserialize<BlogPostRequest>(jsonData);
        var image = form.Files.GetFile("image");
        return await postService.UpdatePostAsync(request, image, cancellationToken);

    }



}
