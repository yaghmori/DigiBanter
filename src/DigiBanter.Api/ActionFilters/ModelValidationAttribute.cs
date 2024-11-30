using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DigiBanter.Api.ActionFilters;

public class ModelValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            List<string> errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList();

            context.Result = new BadRequestObjectResult(errors); // returns 400 with error
        }

    }
}
