﻿using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Extensions;
using DigiBanter.Shared.ResultWrapper;
using DigiBanter.Shared.Services;
using System.Text;

namespace DigiBanter.Application.ApiServices.PodcastApiService
{

    public class PodcastApiService : BaseHttpClient, IPodcastApiService
    {
        private readonly ITimeZoneHelper _timeZoneService;

        public PodcastApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
            : base(httpClient, timeZoneService, jsonService)
        {
            _timeZoneService = timeZoneService;
        }

        public async Task<IApiResult<List<PodcastItem>>> GetPodcastsAsync(CancellationToken cancellationToken = default)
        {
            await AddTimeZoneHeader(cancellationToken);
            var uri = AppEndPoints.Podcast.Base;
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<List<PodcastItem>>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<PodcastItem>> GetTenantByIdAsync(Guid podcastId, CancellationToken cancellationToken = default)
        {
            await AddTimeZoneHeader(cancellationToken);

            var uri = string.Format(AppEndPoints.Podcast.GetById, podcastId, cancellationToken);
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<PodcastItem>(_jsonService, cancellationToken);
        }
    }
}