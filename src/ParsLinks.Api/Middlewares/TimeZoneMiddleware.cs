using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Services.TimeZoneResolver;
using System.Security.Claims;

namespace ParsLinks.Api.Middlewares;
public class TimeZoneMiddleware
{
    private readonly RequestDelegate _next;

    public TimeZoneMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITimeZoneProvider timeZoneProvider)
    {
        string timeZoneId = context.Request.Headers[AppConstants.TimeZoneId].FirstOrDefault() ?? TimeZoneInfo.Local.Id;
        timeZoneProvider.SetTimeZone(timeZoneId);

        context.User.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(AppConstants.TimeZoneId, timeZoneId) }));

        await _next(context);

    }
}
