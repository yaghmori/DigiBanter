

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.Models;

public class BlogPostService : IBlogPostService
{
    private readonly AppDbContext _appDbContext;
    private readonly AppConfig _config;
    private readonly IMapper _mapper;

    public BlogPostService(AppDbContext appDbContext,
        IConfiguration config,
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _config = config.Get<AppConfig>()!;
        _mapper = mapper;
    }

    public async Task<ServiceResult<int>> AddPostAsync(IFormFile image, BlogPostRequest request, HttpContext context, CancellationToken cancellationToken)
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


    public async Task<ServiceResult<List<BlogPostResponse>>> GetPostAsync(HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var baseUrl = _config.Endpoints.Api;
        var posts = await _appDbContext.PostTranslations
            .Include(x => x.Post).ThenInclude(x => x.Author)
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{lang}%"))
            .ProjectTo<BlogPostResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();
        foreach (var item in posts)
        {
            item.Image = $"{baseUrl}{item.Image}";
        }

        return ServiceResult<List<BlogPostResponse>>.Success(posts);
    }


    public async Task<ServiceResult<BlogPostResponse>> GetPostByIdAsync(int Id, HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var baseUrl = _config.Endpoints.Api;

        var post = await _appDbContext.PostTranslations
            .Where(x => x.Post.Id == Id)
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{lang}%"))
            .ProjectTo<BlogPostResponse>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return ServiceResult<BlogPostResponse>.Failure("Post not found", statusCode: 404);

        post.Image = $"{baseUrl}{post.Image}";

        return ServiceResult<BlogPostResponse>.Success(post);
    }




}


