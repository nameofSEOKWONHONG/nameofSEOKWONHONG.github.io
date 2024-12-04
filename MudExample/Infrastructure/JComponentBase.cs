using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudExample.Infrastructure;

public class JComponentBase<TParameter> : ComponentBase
{
    [CascadingParameter] public IMudDialogInstance DialogInstance { get; set; }

    public void ClickOk(TParameter parameter)
    {
        DialogInstance.Close(DialogResult.Ok(parameter));
    }
}