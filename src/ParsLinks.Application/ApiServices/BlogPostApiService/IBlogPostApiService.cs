using ParsLinks.Shared.Dto;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface IBlogPostApiService
{
    Task<IApiResult<int>> AddPostAsync(FileDto image, BlogPostRequest request, CancellationToken cancellationToken = default);
    Task<IApiResult<List<BlogPostResponse>>> GetAllPostsAsync(string? lang = "en-US", CancellationToken cancellationToken = default);
    Task<IApiResult<BlogPostResponse>> GetPostByIdAsync(int postId, string? lang = "en-US", CancellationToken cancellationToken = default);

}