
namespace ParsLinks.Shared.Services
{
    public interface ISnackbarService
    {
        void DisplayWarning(List<string>? messages);
        void DisplayError(List<string>? messages);
        void DisplayWarning(string? message);
        void DisplayError(string? message);
        void DisplaySuccess(List<string>? messages);
        void DisplaySuccess(string? message);
    }
}
