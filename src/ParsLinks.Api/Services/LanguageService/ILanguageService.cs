using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;

public interface ILanguageService
{
    Task<ServiceResult<List<LanguageResponse>>> GetAllAsync(CancellationToken cancellationToken);
}