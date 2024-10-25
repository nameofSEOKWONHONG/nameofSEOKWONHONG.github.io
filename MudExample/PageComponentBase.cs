using System.Runtime.InteropServices;
using Blazored.LocalStorage;
using eXtensionSharp;
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
    [Inject] protected ILocalizer Localizer { get; set; }
    protected bool ProgressVisible { get; set; }
    protected List<BreadcrumbItem> Breadcrumbs = new();

    protected override void OnInitialized()
    {
        var items = NavigationManager.Uri.xSplit("/");
        foreach (var item in items)
        {
            if(item.Contains("http") || item.Contains("localhost") || item.Contains("nameofseokwonhong.github.io")) continue;
            if(Breadcrumbs.Count <= 0)
                Breadcrumbs.Add(new BreadcrumbItem(item.ToUpper(), $"/{item}", true));
            else
                Breadcrumbs.Add(new BreadcrumbItem(item.ToUpper(), $"#", true));
        }
    }

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