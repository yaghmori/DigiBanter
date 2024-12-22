using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ParsLinks.Shared.Services
{
    public interface IDialogService
    {
        IDialogReference ShowDialog<T>() where T : ComponentBase;
        IDialogReference ShowDialog<T>(DialogOptions options) where T : ComponentBase;
        IDialogReference ShowDialog<T>(DialogOptions options, string title = "") where T : ComponentBase;
        IDialogReference ShowDialog<T>(Dictionary<string, object> parameters, DialogOptions options, string title = "") where T : ComponentBase;
        IDialogReference ShowDialog<T>(Dictionary<string, object> parameters, string title = "") where T : ComponentBase;

        Task<DialogResult?> ShowMessageBoxAsync(string body,
            string title = "",
            string btnCancel = "Cancel",
            string titleIcon = "",
            string btnOk = "OK",
            string btnColor = "",
            string iconColor = "Inherit",
            string btnIcon = "");

        Task<bool> ShowDeleteMessageBoxAsync(string body, string title = "Delete Confirmation", string btnOk = "Delete");
        Task<bool> ShowConfirmationDialogAsync(string body, string title = "Info", string btnOk = "Submit");
        Task<DialogResult?> ShowSuccessMessageBoxAsync(string body, string title = "Success");

    }
}
