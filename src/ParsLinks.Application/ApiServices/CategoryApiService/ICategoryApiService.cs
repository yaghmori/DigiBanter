using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface ICategoryApiService
{
    Task<IApiResult<List<CategoryResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

}