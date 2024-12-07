using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface IPodcastApiService
{
    Task<IApiResult<List<PodcastResponse>>> GetPodcastsAsync(CancellationToken cancellationToken = default);
    Task<IApiResult<PodcastResponse>> GetPodcastByIdAsync(Guid podcastId, CancellationToken cancellationToken = default);

}

