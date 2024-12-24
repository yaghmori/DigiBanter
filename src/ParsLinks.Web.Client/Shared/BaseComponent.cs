using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ParsLinks.Web.Client.Shared;

public abstract class BaseComponent : ComponentBase, IDisposable
{
    //[Inject]
    //public IMainHubClient _mainHubClient { get; set; }
    private CancellationTokenSource? cancellationTokenSource;

    public CancellationToken cancellationToken => (cancellationTokenSource ??= new()).Token;


    [CascadingParameter] public MudDialogInstance? _mudDialog { get; set; }
    [CascadingParameter(Name = "Language")] public string? Language { get; set; }
    [Parameter] public string? lang { get; set; }
    [Parameter] public string ReturnUrl { get; set; } = string.Empty;
    [Parameter] public bool IsBusy { get; set; } = false;

    [Parameter] public string Label { get; set; } = string.Empty;

    [Parameter] public bool Clearable { get; set; } = false;

    [Parameter] public string Class { get; set; } = "";

    [Parameter] public string Style { get; set; } = "";

    [Parameter]
    public bool IsReadOnly { get; set; } = false;

    [Parameter]
    public bool IsDisabled { get; set; } = false;

    [Parameter]
    public bool IsAuthorized { get; set; } = true;

    public bool IsLoading { get; set; } = false;
    public bool IsSaving { get; set; } = false;
    public bool HasInit { get; set; } = false;

    public virtual void Dispose()
    {

        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
    public virtual void CancelDialog() => _mudDialog?.Cancel();

}
