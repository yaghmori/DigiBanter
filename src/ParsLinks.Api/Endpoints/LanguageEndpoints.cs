using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

public static class LanguageEndpoints
{
    public static void MapLanguageEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup(AppEndPoints.LanguageEndpoints.Base);


        group.MapGet("/", GetAll).AllowAnonymous();
    }





    private static async Task<IResult> GetAll(ILanguageService languageService,CancellationToken cancellationToken)
    {
        var result = await languageService.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<List<LanguageResponse>>.Success(result.Data));
    }
}
