using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;

public interface ICategoryService
{
    Task<IResult> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default!);
    Task<IResult> UpdateAsync(CategoryRequest request, CancellationToken cancellationToken = default!);
    Task<IResult> GetAllAsync(CategoryQueryParameters parameters, CancellationToken cancellationToken = default!);
    Task<IResult> GetDetailByIdAsync(int categoryId, CancellationToken cancellationToken = default!);
    Task<IResult> DeleteAsync(int categoryId, CancellationToken cancellationToken);
}