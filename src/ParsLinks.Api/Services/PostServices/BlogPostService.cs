

using AutoMapper;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Azure.Core;
using AutoMapper.QueryableExtensions;
using System.Buffers.Text;
using ParsLinks.Domain.Entities;

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


