using DigiBanter.Shared.Dto.Response;
using DigiBanter.Shared.ResultWrapper;

namespace DigiBanter.Application.ApiServices;

public interface IBlogPostApiService
{
    Task<IApiResult<List<BlogPostResponse>>> GetAllPostsAsync(string? lang= "en-US", CancellationToken cancellationToken = default);
    Task<IApiResult<BlogPostResponse>> GetPostByIdAsync(int postId, string? lang = "en-US", CancellationToken cancellationToken = default);

}