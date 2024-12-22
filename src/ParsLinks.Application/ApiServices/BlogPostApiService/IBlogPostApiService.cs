using ParsLinks.Domain.Enums;
using ParsLinks.Shared.Dto;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface IBlogPostApiService
{
    Task<IApiResult<int>> AddPostAsync(FileDto image, BlogPostRequest request, CancellationToken cancellationToken = default);
    Task<IApiResult> UpdatePostAsync(FileDto? image, BlogPostRequest request, CancellationToken cancellationToken = default);
    Task<IApiResult<List<BlogPostResponse>>> GetAllPostsAsync(BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IApiResult<IPagedList<BlogPostResponse>>> GetPagedAllPostsAsync(BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IApiResult<BlogPostResponse>> GetPostByIdAsync(int postId, BlogPostQueryParameters? parameter = null, CancellationToken cancellationToken = default);
    Task<IApiResult<BlogPostRequest>> GetPostByDetailIdAsync(int postId, CancellationToken cancellationToken = default);
    Task<IApiResult> DeletePostByIdAsync(int postId, CancellationToken cancellationToken = default);
    Task<IApiResult> ChangePostStatusAsync(int postId, BlogPostStatusEnum status, CancellationToken cancellationToken = default);

}