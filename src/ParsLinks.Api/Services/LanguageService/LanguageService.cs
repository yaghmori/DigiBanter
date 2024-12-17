using AutoMapper;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Shared.Dto.Response;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

public class LanguageService : ILanguageService
{
    private readonly AppDbContext _appDbContext;
    private readonly IMapper _mapper;

    public LanguageService(AppDbContext appDbContext,
        IMapper mapper)
    {
        _appDbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<ServiceResult<List<LanguageResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var langs = await _appDbContext.Languages
            .ProjectTo<LanguageResponse>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return ServiceResult<List<LanguageResponse>>.Success(langs);
    }

}


