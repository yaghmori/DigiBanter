using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Services;
using System.Net.Http;
using System.Text.Json;

namespace DigiBanter.Application;

public class BaseHttpClient
{
    public readonly HttpClient _httpClient;
    private readonly ITimeZoneHelper _timeZoneService;
    public readonly IJsonSerializer _jsonService;

    public BaseHttpClient(IHttpClientFactory httpClientFactory, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
    {
        _httpClient = httpClientFactory.CreateClient(AppClientTypes.Master);
        _timeZoneService = timeZoneService;
        _jsonService = jsonService;
    }

    protected virtual async Task AddTimeZoneHeader(CancellationToken cancellationToken = default)
    {
        try
        {


            var timeZone = await _timeZoneService.GetUserTimeZone();
            _httpClient.DefaultRequestHeaders.Remove(AppConstants.TimeZoneId);
            _httpClient.DefaultRequestHeaders.Add(AppConstants.TimeZoneId, timeZone);
        }
        catch
        {
            //logger
        }
    }


}