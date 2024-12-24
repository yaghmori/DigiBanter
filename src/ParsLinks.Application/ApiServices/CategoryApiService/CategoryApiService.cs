using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;
using System.Text;


namespace ParsLinks.Application.ApiServices;


public class CategoryApiService : BaseHttpClient, ICategoryApiService
{
    private readonly ITimeZoneHelper _timeZoneService;

    public CategoryApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
        : base(httpClient, timeZoneService, jsonService)
    {
        _timeZoneService = timeZoneService;
    }

    public async Task<IApiResult<int>> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.CategoryEndpoints.AddCategory;
        var content = new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(uri, content, cancellationToken);
        return await response.ToResultAsync<int>(_jsonService, cancellationToken);
    }

    public async Task<IApiResult> UpdateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.CategoryEndpoints.UpdateCategory;
        var content = new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(uri, content, cancellationToken);
        return await response.ToResultAsync(_jsonService, cancellationToken);
    }
    public async Task<IApiResult<List<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.CategoryEndpoints.Base;


        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<List<CategoryResponse>>(_jsonService, cancellationToken);
    }

    public async Task<IApiResult<IPagedList<CategoryResponse>>> GetPagedAllAsync(CategoryQueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
            parameters = new();

        parameters.Paged = true;
        var queryString = parameters.ToQueryString();
        var uri = $"{AppEndPoints.CategoryEndpoints.Base}{queryString}";
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<PagedList<CategoryResponse>>(_jsonService, cancellationToken);
    }
    public async Task<IApiResult<IVirtualizedList<CategoryResponse>>> GetVirtualizedAllAsync(CategoryQueryParameters? parameters = null, CancellationToken cancellationToken = default)
    {
        if (parameters == null)
            parameters = new();

        var queryString = parameters.ToQueryString();
        var uri = $"{AppEndPoints.CategoryEndpoints.Base}{queryString}";
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<VirtualizedList<CategoryResponse>>(_jsonService, cancellationToken);
    }


    public async Task<IApiResult<CategoryRequest>> GetDetailByIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(AppEndPoints.CategoryEndpoints.GetDetailById, categoryId);
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<CategoryRequest>(_jsonService, cancellationToken);
    }
    public async Task<IApiResult> DeleteByIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(AppEndPoints.CategoryEndpoints.DeleteById, categoryId);
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);
        return await response.ToResultAsync(_jsonService, cancellationToken);
    }

}
