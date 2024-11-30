
namespace DigiBanter.Shared.Services;

public interface ITimeZoneHelper
{
    Task<string> GetUserTimeZone();
}