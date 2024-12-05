using Blazored.LocalStorage;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudExample.Data;

namespace MudExample.Components.Base;

public abstract class PageComponentBase : MudComponentBase, IAsyncDisposable
{
    /// <summary>
    /// MudDialog Tag를 선언해야만 사용할 수 있습니다.
    /// </summary>
    [CascadingParameter] protected IMudDialogInstance MudDialog { get; set; }
    
    /// <summary>
    /// DataGrid 및 Table용 MultiSelection 처리를 위한 파라메터 입니다.
    /// </summary>
    [Parameter] public bool IsMultiSelection { get; set; } = true;
    [Parameter] public RenderFragment ChildContent {get;set;}
    
    [Inject] protected IDialogService DialogService { get; set; }
    [Inject] protected ISnackbar Snackbar { get; set; }
    [Inject] protected ILocalStorageService LocalStorageService { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected ILocalizer Localizer { get; set; }
    [Inject] private MenuViewModel MenuViewModel { get; set; }
    
    protected bool ProgressVisible { get; set; }
    protected List<BreadcrumbItem> Breadcrumbs = new();
    
    #region [intialize]

    protected sealed override async Task OnInitializedAsync()
    {
        this.Logger.LogInformation("OnInitializedAsync");
        
        var items = MenuViewModel.SelectedHref.xSplit("/");
        
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

    protected virtual async Task OnRetrieve(Func<Task<Result>> func)
    {   
        await Progress(func);
    }
    
    protected virtual async Task OnCreate(Func<Task<Result>> func)
    {
        await Progress(func);
    }
    
    protected virtual async Task OnUpdate(Func<Task<Result>> func)
    {
        await Progress(func);
    }    

    protected virtual async Task OnDelete(Func<Task<Result>> func)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Warning", 
            "Deleting can not be undone!", 
            yesText:"Delete!", cancelText:"Cancel");
        if(result == null) return;
        
        await Progress(func);
    }
    
    #endregion
    
    private async Task Progress(Func<Task<Result>> func)
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
        
        var result = await func();
        await Task.Delay(500);
        
        this.Snackbar.Add(result.Message, result.Succeed ? Severity.Success : Severity.Error);
        
        dialog.Close();
    }

    #region [simple dialog]

    protected async Task<bool?> ShowDialogAsync(string title, string message)
    {
        return await DialogService.ShowMessageBox(title, message);
    }

    protected async Task<bool?> ShowDialogAsync(string title, string message, string yesText, string noText)
    {
        return await DialogService.ShowMessageBox(title, message, yesText, noText);
    }

    protected async Task<bool?> ShowDialogAsync(string title, string message, string yesText, string noText,
        string cancelText)
    {
        return await DialogService.ShowMessageBox(title, message, yesText, noText, cancelText);
    }

    #endregion
    

    public virtual ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}