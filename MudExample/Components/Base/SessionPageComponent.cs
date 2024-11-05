using eXtensionSharp;
using MudExample.Data;

namespace MudExample.Components.Base;

public abstract class SessionPageComponent : PageComponentBase
{
    private bool _allowRemove;
    protected string RoleName { get; set; }

    protected override async Task OnDelete(Func<Task<Result>> func)
    {
        this.Logger.LogDebug(this.RoleName);
        var allowRemove = this.RoleName.xValue<string>("guest").ToUpper() == "Administrator".ToUpper();
        this.Logger.LogDebug($"allowRemove:{allowRemove}");
        if (!allowRemove)
        {
            await this.DialogService.ShowMessageBox("Permission Problems", "You don't have permission to delete.");
            return;
        }
        await base.OnDelete(func);
    }
}