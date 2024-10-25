using System.Runtime.InteropServices;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExample.Data;
using MudExample.Pages;

namespace MudExample;

public abstract class PageComponentBase : ComponentBase
{
    [Parameter] public RenderFragment ChildContent {get;set;}
    [Inject] protected IDialogService DialogService { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected ILocalStorageService LocalStorageService { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected Localizer Localizer { get; set; }
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
        if(result == null) return;
        
        this.ShowProgress();
        await func();
        this.CloseProgress();
    }
}