using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudExample;

public abstract class PageComponentBase : ComponentBase
{
    [Parameter] public RenderFragment ChildContent {get;set;}
    [Inject] private IDialogService DialogService { get; set; }
    protected bool ProgressVisible { get; set; }

    protected void ShowProgress()
    {
        this.ProgressVisible = true;
        this.StateHasChanged();
    }

    protected void CloseProgress()
    {
        this.ProgressVisible = false;
        this.StateHasChanged();
    }

    protected virtual async Task Remove(Func<Task> func)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", 
            "Deleting can not be undone!", 
            yesText:"Delete!", cancelText:"Cancel");
        var state = result == null ? "Canceled" : "Deleted!";
        this.ShowProgress();
        await func();
        this.CloseProgress();
    }
}