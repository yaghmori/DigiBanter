using DigiBanter.Shared.Dto.Response;
using DigiBanter.Shared.ResultWrapper;

namespace DigiBanter.Application.ApiServices;

public interface IPodcastApiService
{
    Task<IApiResult<List<PodcastResponse>>> GetPodcastsAsync(CancellationToken cancellationToken = default);
    Task<IApiResult<PodcastResponse>> GetPodcastByIdAsync(Guid podcastId, CancellationToken cancellationToken = default);

}

