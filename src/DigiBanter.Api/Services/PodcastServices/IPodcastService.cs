using DigiBanter.Api.Services;
using DigiBanter.Shared.Dto.Response;

public interface IPodcastService
{
    Task<ServiceResult<List<PodcastResponse>>> GetAllAsync(HttpContext context, CancellationToken cancellationToken, string? lang = "en-US");
    Task<ServiceResult<PodcastResponse>> GetByIdAsync(Guid id, HttpContext context, CancellationToken cancellationToken, string? lang = "en-US");
}