using Microsoft.Identity.Client;
using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;

public interface IPodcastService
{
    Task<ServiceResult<List<PodcastResponse>>> GetAllAsync(HttpContext context,AppConfig appConfig, CancellationToken cancellationToken, string? lang = "en-US");
    Task<ServiceResult<PodcastResponse>> GetByIdAsync(Guid id, HttpContext context, AppConfig appConfig, CancellationToken cancellationToken, string? lang = "en-US");
}