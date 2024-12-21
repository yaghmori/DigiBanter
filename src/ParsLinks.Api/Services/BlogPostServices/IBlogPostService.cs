using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Models;

public interface IBlogPostService
{
    Task<ServiceResult<int>> AddPostAsync(IFormFile image, BlogPostRequest request, CancellationToken cancellationToken);
    Task<IResult> GetPostAsync(BlogPostQueryParameters parameters, CancellationToken cancellationToken);
    Task<IResult> GetPostByIdAsync(int postId, BlogPostQueryParameters parameters, CancellationToken cancellationToken);
    Task<IResult> DeletePostAsync(int postId, CancellationToken cancellationToken);
    Task<IResult> UpdatePostAsync(BlogPostRequest request, IFormFile? image, CancellationToken cancellationToken);
    Task<IResult> GetPostDetailByIdAsync(int postId, CancellationToken cancellationToken);
}