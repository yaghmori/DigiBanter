

using AutoMapper;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using Microsoft.EntityFrameworkCore;

public class BlogPostService : IBlogPostService
{
    private readonly AppDbContext _appDbContext;
    private readonly AppConfig _config;
    private readonly IMapper _mapper;
    private readonly IJsonSerializer _jsonSerializer;

    public BlogPostService(AppDbContext appDbContext,
        IConfiguration config,
        IMapper mapper,
        IJsonSerializer jsonSerializer)
    {
        _appDbContext = appDbContext;
        _config = config.Get<AppConfig>()!;
        _mapper = mapper;
        _jsonSerializer = jsonSerializer;
    }

    public async Task<ServiceResult<List<BlogPostResponse>>> GetPostAsync(HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var posts = await _appDbContext.PostTranslations
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{lang}%"))
            .Select(x => new BlogPostResponse
            {
                Id = x.Post.Id,
                Author = x.Post.Author.DisplayName,
                Image = x.Post.Image,
                Title = x.Title,
                Content = x.Content.Length > 1000 ? x.Content.Substring(0, 1000) + "..." : x.Content, // Truncate and append "..."
                Slug = x.Slug,
                Language = x.Language.Code,
                PublishedAt = x.Post.PublishedAt.Value.Date
            }).ToListAsync();

        return ServiceResult<List<BlogPostResponse>>.Success(posts);
    }


    public async Task<ServiceResult<BlogPostResponse>> GetPostByIdAsync(int Id,HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var post = await _appDbContext.PostTranslations
            .Where(x => x.Post.Id == Id)
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{lang}%"))
            .Select(x => new BlogPostResponse
            {
                Id = x.Post.Id,
                Author = x.Post.Author.DisplayName,
                Image = x.Post.Image,
                Title = x.Title,
                Content = x.Content,
                Slug = x.Slug,
                Language = x.Language.Code,
                PublishedAt = x.Post.PublishedAt.Value.Date
            }).FirstOrDefaultAsync(cancellationToken);

        if (post == null)
            return ServiceResult<BlogPostResponse>.Failure("Post not found",statusCode:404);

        return ServiceResult<BlogPostResponse>.Success(post);
    }





}


