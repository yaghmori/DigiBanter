using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Shared.Dto.Response;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext appDbContext,
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<CategoryResponse>>> GetAllAsync(string? lang = "en-US", CancellationToken cancellationToken = default!)
    {
        var categories = await _appDbContext.CategoryTranslations
            .Where(x => EF.Functions.Like(x.Language.Code, $"%{lang}%"))
            .ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return ServiceResult<List<CategoryResponse>>.Success(categories);
    }
}


