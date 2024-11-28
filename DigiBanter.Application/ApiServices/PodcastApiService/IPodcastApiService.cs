
using DigiBanter.Shared.ResultWrapper;

namespace DigiBanter.Application.ApiServices.PodcastApiService
{
    public interface IPodcastApiService
    {
        Task<IApiResult<List<PodcastItem>>> GetPodcastsAsync(CancellationToken cancellationToken = default);
        Task<IApiResult<PodcastItem>> GetTenantByIdAsync(Guid podcastId, CancellationToken cancellationToken = default);

    }
}