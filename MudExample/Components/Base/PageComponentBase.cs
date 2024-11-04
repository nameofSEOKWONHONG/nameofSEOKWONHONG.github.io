using Blazored.LocalStorage;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudExample.Data;

namespace MudExample.Components.Base;

public abstract class PageComponentBase : ComponentBase, IAsyncDisposable
{
    [Parameter] public RenderFragment ChildContent {get;set;}
    [Inject] protected IDialogService DialogService { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected ILocalStorageService LocalStorageService { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected ILocalizer Localizer { get; set; }
    [Inject] protected ILogger<PageComponentBase> Logger { get; set; }
    
    protected bool ProgressVisible { get; set; }
    protected List<BreadcrumbItem> Breadcrumbs = new();

    #region [intialize]

    protected override async Task OnInitializedAsync()
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
        
        await OnInitViewData();
        await OnInitRetrieve();
    }
    

    /// <summary>
    /// 화면에 필요한 UI 구성요소 데이터 조회
    /// </summary>
    /// <returns></returns>
    protected virtual async Task OnInitViewData()
    {
        Logger.LogInformation("OnViewData is retrieve page component data. Example, get select list item from server.");
        await Task.Delay(200);
    }

    /// <summary>
    /// 조회 데이터 조회
    /// </summary>
    /// <returns></returns>
    protected virtual async Task OnInitRetrieve()
    {
        Logger.LogInformation("OnRetrieve is get data from server.");
        await Task.Delay(200);
    }    

    #endregion

    #region [click events]

    protected virtual async Task OnRetrieve(Func<Task> func)
    {   
        await Progress(func);
    }
    
    protected virtual async Task OnCreate(Func<Task> func)
    {
        await Progress(func);
    }
    
    protected virtual async Task OnUpdate(Func<Task> func)
    {
        await Progress(func);
    }    

    protected virtual async Task OnDelete(Func<Task> func)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", 
            "Deleting can not be undone!", 
            yesText:"Delete!", cancelText:"Cancel");
        if(result == null) return;
        
        await Progress(func);
    }
    
    #endregion
    
    private async Task Progress(Func<Task> func)
    {
        if (func.xIsEmpty())
        {
            Logger.LogInformation("Task function is empty.");
            return;
        }
        
        var options = new DialogOptions
        {
            CloseOnEscapeKey = false, 
            BackdropClick = false,
            NoHeader = true
        };

        // Show the dialog
        var dialog = await DialogService.ShowAsync<ProgressDialog>("Progress", options);
        
        await func();
        
        dialog.Close();
    }

    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}