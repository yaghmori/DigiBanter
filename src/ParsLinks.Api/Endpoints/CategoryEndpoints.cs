using Microsoft.AspNetCore.Mvc;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup(AppEndPoints.CategoryEndpoints.Base);


        group.MapGet("/", GetAll).AllowAnonymous();
        group.MapGet("/{categoryId}/detail", GetDetailById).AllowAnonymous();
        group.MapPost("/", AddCategory).AllowAnonymous();
        group.MapPatch("/", UpdateCategory).AllowAnonymous();
        group.MapDelete("/{categoryId}", DeletePostById).AllowAnonymous();

    }


    private static async Task<IResult> AddCategory(
      [FromBody] CategoryRequest request,
      ICategoryService categoryService,
      CancellationToken cancellationToken = default!)
    {
        return await categoryService.AddAsync(request, cancellationToken);

    }

    private static async Task<IResult> UpdateCategory(
        [FromBody] CategoryRequest request,
        ICategoryService categoryService,
        CancellationToken cancellationToken = default!)
    {
        return await categoryService.UpdateAsync(request, cancellationToken);

    }


    private static async Task<IResult> DeletePostById(
  [FromRoute] int categoryId,
  ICategoryService categoryService,
  CancellationToken cancellationToken)
    {
        return await categoryService.DeleteAsync(categoryId, cancellationToken);
    }
    private static async Task<IResult> GetAll(ICategoryService categoryService,
        [AsParameters] CategoryQueryParameters parameters,
        CancellationToken cancellationToken = default!)
    {
        return await categoryService.GetAllAsync(parameters, cancellationToken);
    }

    private static async Task<IResult> GetDetailById(
        [FromRoute] int categoryId,
        ICategoryService categoryService,
        CancellationToken cancellationToken = default!)
    {
        return await categoryService.GetDetailByIdAsync(categoryId, cancellationToken);

    }
}
