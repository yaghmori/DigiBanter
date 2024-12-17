using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;


namespace ParsLinks.Application.ApiServices;


public class CategoryApiService : BaseHttpClient, ICategoryApiService
{
    private readonly ITimeZoneHelper _timeZoneService;

    public CategoryApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
        : base(httpClient, timeZoneService, jsonService)
    {
        _timeZoneService = timeZoneService;
    }

    public async Task<IApiResult<List<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.CategoryEndpoints.Base;


        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<List<CategoryResponse>>(_jsonService, cancellationToken);
    }


}
