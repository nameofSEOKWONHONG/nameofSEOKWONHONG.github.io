using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using MudExample;
using MudExample.Data;
using OllamaSharp;

#if DEBUG

// Allow some time for debugger to attach to Blazor framework debugging proxy
await Task.Delay(TimeSpan.FromSeconds(2));
#endif

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;    
});

builder.Services.AddScoped<IOllamaApiClient, OllamaApiClient>(sp => new OllamaApiClient("http://localhost:11434/"));
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton<MenuViewModel>();
builder.Services.AddSingleton<Localizer>();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddLocalization();

var app = builder.Build();
using var scope = app.Services.CreateScope();
var localizer = scope.ServiceProvider.GetRequiredService<Localizer>();
var localStorage = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

var cultureString = await localStorage.GetItemAsync<string>("culture");
CultureInfo cultureInfo;
if (!string.IsNullOrWhiteSpace(cultureString))
{
    cultureInfo = new CultureInfo(cultureString);
}
else
{
    cultureInfo = new CultureInfo("en-US");
    cultureString = "en-US";
}
 
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
await localizer.InitializeAsync(cultureString);

await app.RunAsync();