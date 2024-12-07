using Microsoft.AspNetCore.Components;

namespace DigiBanter.Auth.Components;

public abstract class BaseComponent : ComponentBase, IDisposable
{
    //[Inject]
    //public IMainHubClient _mainHubClient { get; set; }
    private CancellationTokenSource? cancellationTokenSource;

    public CancellationToken cancellationToken => (cancellationTokenSource ??= new()).Token;
    [SupplyParameterFromQuery] public string ReturnUrl { get; set; } = string.Empty;
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

}
