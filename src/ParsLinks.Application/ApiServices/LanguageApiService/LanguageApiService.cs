using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;


namespace ParsLinks.Application.ApiServices
{

    public class LanguageApiService : BaseHttpClient, ILanguageApiService
    {
        private readonly ITimeZoneHelper _timeZoneService;

        public LanguageApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
            : base(httpClient, timeZoneService, jsonService)
        {
            _timeZoneService = timeZoneService;
        }

        public async Task<IApiResult<List<LanguageResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var uri = AppEndPoints.LanguageEndpoints.Base;


            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<List<LanguageResponse>>(_jsonService, cancellationToken);
        }


    }
}
