using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;

public interface IBlogPostService
{
    Task<ServiceResult<List<BlogPostResponse>>> GetPostAsync(HttpContext context, CancellationToken cancellationToken, string? lang = "en-US");
    Task<ServiceResult<BlogPostResponse>> GetPostByIdAsync(int id, HttpContext context, CancellationToken cancellationToken, string? lang = "en-US");
}