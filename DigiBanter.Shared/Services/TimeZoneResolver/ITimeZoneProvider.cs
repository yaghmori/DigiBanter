namespace DigiBanter.Shared.Services.TimeZoneResolver;

public interface ITimeZoneProvider
{
    public TimeZoneInfo CurrentTimeZone { get; }
    public void SetTimeZone(TimeZoneInfo timeZone);
    public void SetTimeZone(string timeZoneId);

}


