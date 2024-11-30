using DigiBanter.Shared.Dto.Response;
using DigiBanter.Shared.ResultWrapper;

namespace DigiBanter.Application.ApiServices;

public interface IPodcastApiService
{
    Task<IApiResult<List<PodcastItem>>> GetPodcastsAsync(CancellationToken cancellationToken = default);
    Task<IApiResult<PodcastItem>> GetPodcastByIdAsync(Guid podcastId, CancellationToken cancellationToken = default);

}

