using System.Globalization;
using Blazor.SubtleCrypto;
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
builder.Services.AddLocalizerAsSingleton();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<LayoutState>();
builder.Services.AddLocalization();
builder.Services.AddSubtleCrypto(opt => opt.Key = "kR0BsODSKxPhAWkKpePGmUTvUygYkb9ijbwjnqezc5P8ICszLGyeeVXZJPbaZuTDq8GvP6O4OC92jAse");

var app = builder.Build();
await app.UseLocalizer();
await app.RunAsync();