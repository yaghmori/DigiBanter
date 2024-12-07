using ParsLinks.Shared.Services;
using Microsoft.JSInterop;

namespace ParsLinks.Web.Client.Services;

public class TimeZoneHelper : ITimeZoneHelper
{
    private readonly IJSRuntime _jsRuntime;

    public TimeZoneHelper(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task<string> GetUserTimeZone()
    {
        var timeZone = await _jsRuntime.InvokeAsync<string>("getTimeZone");
        return !string.IsNullOrEmpty(timeZone) ? timeZone : "UTC"; // Default to UTC if time zone is null or empty
    }
}
