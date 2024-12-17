using Microsoft.AspNetCore.Mvc;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.ResultWrapper;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup(AppEndPoints.CategoryEndpoints.Base);


        group.MapGet("/", GetAll).AllowAnonymous();
    }





    private static async Task<IResult> GetAll(ICategoryService categoryService, [FromQuery] string? lang = "en-US", CancellationToken cancellationToken = default!)
    {
        var result = await categoryService.GetAllAsync(lang, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<List<CategoryResponse>>.Success(result.Data));
    }
}
