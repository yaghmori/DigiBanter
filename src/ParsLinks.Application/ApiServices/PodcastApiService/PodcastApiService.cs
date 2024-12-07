using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;
using System.Text;

namespace ParsLinks.Application.ApiServices
{

    public class PodcastApiService : BaseHttpClient, IPodcastApiService
    {
        private readonly ITimeZoneHelper _timeZoneService;

        public PodcastApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
            : base(httpClient, timeZoneService, jsonService)
        {
            _timeZoneService = timeZoneService;
        }

        public async Task<IApiResult<List<PodcastResponse>>> GetPodcastsAsync(CancellationToken cancellationToken = default)
        {
            await AddTimeZoneHeader(cancellationToken);
            var uri = AppEndPoints.Podcast.Base;
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<List<PodcastResponse>>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<PodcastResponse>> GetPodcastByIdAsync(Guid podcastId, CancellationToken cancellationToken = default)
        {
            await AddTimeZoneHeader(cancellationToken);

            var uri = string.Format(AppEndPoints.Podcast.GetById, podcastId, cancellationToken);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<PodcastResponse>(_jsonService, cancellationToken);
        }
    }
}
