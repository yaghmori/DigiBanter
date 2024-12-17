using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

namespace ParsLinks.Application.ApiServices;

public interface ILanguageApiService
{
    Task<IApiResult<List<LanguageResponse>>> GetAllAsync(CancellationToken cancellationToken = default);

}