using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;

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


    public async Task<IResult> AddAsync(CategoryRequest request, CancellationToken cancellationToken = default!)
    {
        var category = _mapper.Map<Category>(request);
        await _appDbContext.Categories.AddAsync(category, cancellationToken);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok(ApiResult<int>.Success(category.Id));
    }
    public async Task<IResult> UpdateAsync(CategoryRequest request, CancellationToken cancellationToken = default!)
    {
        if (request == null || request.Id == null)
            return TypedResults.BadRequest("Invalid request");

        var category = await _appDbContext.Categories
            .Include(b => b.Translations)
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);


        // Update translations
        foreach (var translation in request.Translations)
        {
            var existingTranslation = category.Translations
                .FirstOrDefault(t => t.LanguageId == translation.LanguageId);

            if (existingTranslation != null)
            {
                // Update existing translation
                existingTranslation.LanguageId = translation.LanguageId;
                existingTranslation.Name = translation.Name;
                existingTranslation.Slug = translation.Slug;
                existingTranslation.Description = translation.Description;
            }
            else
            {
                // Add new translation
                category.Translations.Add(new CategoryTranslation
                {
                    CategoryId = request.Id.Value,
                    LanguageId = translation.LanguageId,
                    Name = translation.Name,
                    Slug = translation.Slug,
                    Description = translation.Description
                });

            }
        }


        await _appDbContext.SaveChangesAsync(cancellationToken);


        return TypedResults.Ok(ApiResult<int>.Success(category.Id));
    }

    public async Task<IResult> DeleteAsync(int categoryId, CancellationToken cancellationToken)
    {
        var category = await _appDbContext.Categories
             .Where(x => x.Id == categoryId)
             .FirstOrDefaultAsync(cancellationToken);

        if (category == null)
            return TypedResults.NotFound("Category not found or assumed to be deleted.");

        _appDbContext.Categories.Remove(category);
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(ApiResult.Success("Category successfully deleted."));
    }

    public async Task<IResult> GetAllAsync(CategoryQueryParameters parameters, CancellationToken cancellationToken = default!)
    {

        var queryable = _appDbContext.CategoryTranslations
          .Where(x => EF.Functions.Like(x.Language.Code, $"%{parameters.Lang ?? "en-US"}%"))
          .OrderBy(x => x.Name)
          .AsQueryable();


        // Parse the query
        if (!string.IsNullOrWhiteSpace(parameters.Query))
        {
            var searchTerms = parameters.Query
                                        .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(term => term.Trim())
                                        .ToList();

            foreach (var term in searchTerms)
            {
                queryable = queryable.Where(x =>
                    EF.Functions.Like(x.Name.ToLower(), $"%{term}%") ||
                    EF.Functions.Like(x.Description.ToLower(), $"%{term}%"));

            }
        }

        var result = queryable.ProjectTo<CategoryResponse>(_mapper.ConfigurationProvider);





        if (parameters.Paged == true)
        {
            var index = parameters.Index ?? 0;
            var size = parameters.Size > 100 ? 100 : parameters.Size ?? 10;

            var response = await result.ToPagedListAsync(index, size, cancellationToken: cancellationToken);
            return TypedResults.Ok(ApiResult<IPagedList<CategoryResponse>>.Success(response));

        }
        else
        {

            if (parameters.Index.HasValue)
            {
                var size = parameters.Size ?? 0;

                var response = await result.ToVirtualizedListAsync(parameters.Index.Value, size, cancellationToken);
                return TypedResults.Ok(ApiResult<IVirtualizedList<CategoryResponse>>.Success(response));

            }
            else
            {
                var response = await result.ToListAsync(cancellationToken);

                return TypedResults.Ok(ApiResult<List<CategoryResponse>>.Success(response));

            }

        }

    }


    public async Task<IResult> GetDetailByIdAsync(int categoryId, CancellationToken cancellationToken = default!)
    {
        var category = await _appDbContext.Categories
            .Include(x => x.Translations)
            .Where(x => x.Id == categoryId)
            .ProjectTo<CategoryRequest>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        if (category == null)
            return TypedResults.NotFound("category not found");

        return TypedResults.Ok(ApiResult<CategoryRequest>.Success(category));
    }

}


