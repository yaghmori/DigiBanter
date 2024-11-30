using DigiBanter.Shared.Constatns;
using Microsoft.JSInterop;


namespace DigiBanter.Web.Client.HttpDelegateHnadlers;

public class TimeZoneDelegateHandler : DelegatingHandler
{
    private readonly IJSRuntime _jSRuntime;

    public TimeZoneDelegateHandler(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var timeZone = await _jSRuntime.InvokeAsync<string>("getTimeZone");
        if (timeZone != null)
        {
            request.Headers.Remove(AppConstants.TimeZoneId);
            request.Headers.Add(AppConstants.TimeZoneId, timeZone);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
