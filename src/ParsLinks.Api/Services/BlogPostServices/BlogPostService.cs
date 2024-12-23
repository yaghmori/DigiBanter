

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Domain.Enums;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services.TimeZoneResolver;

public class BlogPostService : IBlogPostService
{
    private readonly AppDbContext _appDbContext;
    private readonly AppConfig _config;
    private readonly IMapper _mapper;
    private readonly ITimeZoneProvider _timeZoneProvider;

    public BlogPostService(AppDbContext appDbContext,
        IConfiguration config,
        IMapper mapper,
        ITimeZoneProvider timeZoneProvider)
    {
        _appDbContext = appDbContext;
        _config = config.Get<AppConfig>()!;
        _mapper = mapper;
        _timeZoneProvider = timeZoneProvider;
    }

    public async Task<ServiceResult<int>> AddPostAsync(IFormFile image, BlogPostRequest request, CancellationToken cancellationToken)
    {
        string? imagePath = null;
        var basePath = "assets/images";
        if (image != null && image.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            var uniqueFileName = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(image.FileName)}";
            imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }

            // Store relative path for database
            imagePath = $"/{basePath}/{uniqueFileName}";
        }
        // Map the BlogPostRequest to the Post entity
        // Add the Post entity to the database
        var postEntity = await _appDbContext.Posts.AddAsync(new Post
        {
            Image = imagePath,
            PublishedAt = DateTime.UtcNow,
            AuthorId = Guid.Parse("00000000-0000-0000-0000-000000000001")
        }, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        // Process each translation in the request
        foreach (var translationRequest in request.Translations)
        {
            // Auto-generate slug if not provided
            if (string.IsNullOrEmpty(translationRequest.Slug) && !string.IsNullOrEmpty(translationRequest.Title))
            {
                translationRequest.Slug = StringExtensionsHelpers.GenerateSlug(translationRequest.Title);
            }

            // Create and add the PostTranslation entity
            var postTranslation = new PostTranslation
            {
                PostId = postEntity.Entity.Id,
                LanguageId = translationRequest.LanguageId,
                Title = translationRequest.Title,
                Slug = translationRequest.Slug,
                Content = translationRequest.Content,
                CreatedDate = DateTime.UtcNow
            };

            await _appDbContext.PostTranslations.AddAsync(postTranslation, cancellationToken);
        }

        // Save all translations
        await _appDbContext.SaveChangesAsync(cancellationToken);

        // Return success with the ID of the first translation
        return ServiceResult<int>.Success(postEntity.Entity.Id);
    }

    public async Task<IResult> UpdatePostAsync(BlogPostRequest request, IFormFile? image, CancellationToken cancellationToken)
    {

        if (request == null || request.Id == null)
            return TypedResults.BadRequest("Invalid request");

        var blogPost = await _appDbContext.Posts
            .Include(b => b.Translations)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (blogPost == null)
            return TypedResults.NotFound("Post not found or assumed to be deleted.");

        string? imagePath = null;
        var basePath = "assets/images";
        if (image != null && image.Length > 0)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique filename
            var uniqueFileName = $"{DateTime.UtcNow.Ticks}{Path.GetExtension(image.FileName)}";
            imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(stream, cancellationToken);
            }

            // Store relative path for database
            imagePath = $"/{basePath}/{uniqueFileName}";
            blogPost.Image = imagePath;

        }

        // Update main properties
        blogPost.AuthorId = request.AuthorId;
        blogPost.CategoryId = request.CategoryId;
        blogPost.Status = request.Status;
        blogPost.PublishedAt = request.PublishedAt;



        // Update translations
        foreach (var translation in request.Translations)
        {
            var existingTranslation = blogPost.Translations
                .FirstOrDefault(t => t.LanguageId == translation.LanguageId);

            if (existingTranslation != null)
            {
                // Update existing translation
                existingTranslation.LanguageId = translation.LanguageId;
                existingTranslation.Title = translation.Title;
                existingTranslation.Slug = translation.Slug;
                existingTranslation.Content = translation.Content;
            }
            else
            {
                // Add new translation
                blogPost.Translations.Add(new PostTranslation
                {
                    PostId = request.Id.Value,
                    LanguageId = translation.LanguageId,
                    Title = translation.Title,
                    Slug = translation.Slug,
                    Content = translation.Content
                });
            }
        }


        await _appDbContext.SaveChangesAsync(cancellationToken);

        // Return success with the ID of the first translation
        return TypedResults.Ok(ApiResult.Success("Post successfully updated."));
    }


    public async Task<IResult> DeletePostAsync(int postId, CancellationToken cancellationToken)
    {
        var post = await _appDbContext.Posts
             .Where(x => x.Id == postId)
             .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return TypedResults.NotFound("Post not found or assumed to be deleted.");

        _appDbContext.Posts.Remove(post);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(ApiResult.Success("Post successfully deleted."));
    }

    public async Task<IResult> GetPostAsync(BlogPostQueryParameters parameters, CancellationToken cancellationToken)
    {


        var queryable = _appDbContext.PostTranslations
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{parameters.Lang ?? "en-US"}%"))
            .Include(x => x.Post).ThenInclude(x => x.Author)
            .Include(x => x.Post).ThenInclude(x => x.Category).ThenInclude(x => x.Translations)
                        //.OrderByDescending(x => x.Post.PublishedAt)
                        .OrderBy(x => x.Post.Id)

            .AsQueryable();

        if (parameters.Status?.ToLower() == "all")
        {
            // If 'all' is provided, don't filter by status
        }
        else if (!string.IsNullOrWhiteSpace(parameters.Status))
        {
            var list = parameters.Status.Split(',').Select(int.Parse).ToList();
            queryable = queryable.Where(x => list.Contains((int)x.Post.Status));
        }
        else
        {
            queryable = queryable.Where(x => x.Post.Status == null); // Filter records where Status is null
        }
        if (parameters.Start != null)
        {
            var date = parameters.Start.ToUtcDateTimeOffset(_timeZoneProvider.CurrentTimeZone);
            queryable = queryable.Where(x => x.Post.PublishedAt == null || x.Post.PublishedAt >= date);
        }

        if (parameters.End != null)
        {
            var date = parameters.End.ToUtcDateTimeOffset(_timeZoneProvider.CurrentTimeZone);
            queryable = queryable.Where(x => x.Post.PublishedAt == null || x.Post.PublishedAt <= date);
        }
        // Parse the query
        if (!string.IsNullOrWhiteSpace(parameters.Query))
        {
            var searchTerms = parameters.Query
                                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(term => term.Trim())
                                        .ToList();

            foreach (var term in searchTerms)
            {
                queryable = queryable.Where(x =>
                    EF.Functions.Like(x.Title.ToLower(), $"%{term}%") ||
                    EF.Functions.Like(x.Content.ToLower(), $"%{term}%"));

            }
        }

        var result = queryable.ProjectTo<BlogPostResponse>(_mapper.ConfigurationProvider);




        var baseUrl = _config.Endpoints.Api;

        if (parameters.Paged == true)
        {
            var index = parameters.Index ?? 0;
            var size = parameters.Size > 100 ? 100 : parameters.Size ?? 10;

            var response = await result.ToPagedListAsync(index, size, cancellationToken: cancellationToken);
            foreach (var item in response.Items)
            {
                item.Image = $"{baseUrl}{item.Image}";
            }
            return TypedResults.Ok(ApiResult<IPagedList<BlogPostResponse>>.Success(response));

        }
        else
        {

            if (parameters.Index.HasValue)
            {
                var size = parameters.Size ?? 0;

                var response = await result.ToVirtualizedListAsync(parameters.Index.Value, size, cancellationToken);
                foreach (var item in response.Items)
                {
                    item.Image = $"{baseUrl}{item.Image}";
                }
                return TypedResults.Ok(ApiResult<IVirtualizedList<BlogPostResponse>>.Success(response));

            }
            else
            {
                var response = await result.ToListAsync(cancellationToken);

                foreach (var item in response)
                {
                    item.Image = $"{baseUrl}{item.Image}";
                }
                return TypedResults.Ok(ApiResult<List<BlogPostResponse>>.Success(response));

            }

        }
    }


    public async Task<IResult> GetPostByIdAsync(int postId, BlogPostQueryParameters parameters, CancellationToken cancellationToken)
    {
        var baseUrl = _config.Endpoints.Api;

        var post = await _appDbContext.PostTranslations
            .Where(x => x.Post.Id == postId)
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{parameters.Lang}%"))
            .ProjectTo<BlogPostResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return TypedResults.NotFound("Post not found or assumed to be deleted.");

        post.Image = $"{baseUrl}{post.Image}";

        return TypedResults.Ok(ApiResult<BlogPostResponse>.Success(post));

    }



    public async Task<IResult> GetPostDetailByIdAsync(int postId, CancellationToken cancellationToken)
    {
        var baseUrl = _config.Endpoints.Api;

        var post = await _appDbContext.Posts
            .Where(x => x.Id == postId)
            .ProjectTo<BlogPostRequest>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return TypedResults.NotFound("Post not found or assumed to be deleted.");

        post.Image = $"{baseUrl}{post.Image}";

        return TypedResults.Ok(ApiResult<BlogPostRequest>.Success(post));

    }

    public async Task<IResult> ChangePostStatusAsync(int postId, BlogPostStatusEnum status, CancellationToken cancellationToken)
    {
        var post = await _appDbContext.Posts
            .Where(x => x.Id == postId)
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return TypedResults.NotFound("Post not found or assumed to be deleted.");

        post.Status = status;
        await _appDbContext.SaveChangesAsync(cancellationToken);

        // Return success with the ID of the first translation
        return TypedResults.Ok(ApiResult.Success("Post status successfully updated."));

    }
}


