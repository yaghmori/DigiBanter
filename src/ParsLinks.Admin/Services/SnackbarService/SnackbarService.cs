using MudBlazor;
using ParsLinks.Shared.Services;

namespace ParsLinks.Admin.Services;


public class SnackbarService : ISnackbarService
{
    private readonly ISnackbar _snackbar;

    public SnackbarService(ISnackbar snackbar)
    {
        _snackbar = snackbar;
    }


    public void DisplaySuccess(List<string>? messages)
    {
        if (messages != null)
        {
            foreach (var message in messages)
            {
                _snackbar.Add(message, Severity.Success);
            }
        }
    }

    public void DisplayWarning(List<string>? messages)
    {
        if (messages != null)
        {
            foreach (var message in messages)
            {
                _snackbar.Add(message, Severity.Warning);
            }
        }
    }

    public void DisplayError(List<string>? messages)
    {
        if (messages != null)
        {
            foreach (var message in messages)
            {
                _snackbar.Add(message, Severity.Error);
            }
        }
    }

    public void DisplayWarning(string? message)
    {
        if (message != null)
        {
            _snackbar.Add(message, Severity.Warning);
        }
    }

    public void DisplayError(string? message)
    {
        if (message != null)
        {
            _snackbar.Add(message, Severity.Error);
        }
    }

    public void DisplaySuccess(string? message)
    {
        if (message != null)
        {
            _snackbar.Add(message, Severity.Success);
        }
    }

}
