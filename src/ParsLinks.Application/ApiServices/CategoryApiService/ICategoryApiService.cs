using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface ICategoryApiService
{
    Task<IApiResult<List<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IApiResult<int>> AddCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<IApiResult> UpdateCategoryAsync(CategoryRequest request, CancellationToken cancellationToken = default);
    Task<IApiResult<IPagedList<CategoryResponse>>> GetPagedAllAsync(CategoryQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IApiResult<IVirtualizedList<CategoryResponse>>> GetVirtualizedAllAsync(CategoryQueryParameters? parameters = null, CancellationToken cancellationToken = default);
    Task<IApiResult<CategoryRequest>> GetDetailByIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<IApiResult> DeleteByIdAsync(int categoryId, CancellationToken cancellationToken = default);
}