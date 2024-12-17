using ParsLinks.Api.Services;
using ParsLinks.Shared.Dto.Response;

public interface ICategoryService
{
    Task<ServiceResult<List<CategoryResponse>>> GetAllAsync(string? lang = "en-US", CancellationToken cancellationToken = default!);
}