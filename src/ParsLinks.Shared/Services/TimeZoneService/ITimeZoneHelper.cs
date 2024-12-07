
namespace ParsLinks.Shared.Services;

public interface ITimeZoneHelper
{
    Task<string> GetUserTimeZone();
}