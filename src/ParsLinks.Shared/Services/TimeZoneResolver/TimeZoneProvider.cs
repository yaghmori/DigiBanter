using System;
using TimeZoneConverter;

namespace ParsLinks.Shared.Services.TimeZoneResolver;

public class TimeZoneProvider : ITimeZoneProvider
{
    public TimeZoneInfo CurrentTimeZone { get; private set; } = TimeZoneInfo.Local;

    public void SetTimeZone(TimeZoneInfo timeZone)
    {
        CurrentTimeZone = timeZone;
    }

    public void SetTimeZone(string timeZoneId)
    {
        TimeZoneInfo timeZoneInfo;
        try
        {
            timeZoneInfo = TZConvert.GetTimeZoneInfo(timeZoneId);
        }
        catch (Exception)
        {
            // Fallback to the system's local time zone
            timeZoneInfo = TimeZoneInfo.Local;
        }
    }
}
