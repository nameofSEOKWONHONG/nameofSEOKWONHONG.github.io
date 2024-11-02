using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Brism;
using KristofferStrube.Blazor.MediaCaptureStreams;
using Magic.IndexedDb.Extensions;
using Magic.IndexedDb.Helpers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using MudExample;
using MudExample.Components;
using MudExample.Data;
using MudExample.Infrastructure;
using MudExample.Services;
using MudExtensions.Services;
using OllamaSharp;
using Toolbelt.Blazor.Extensions.DependencyInjection;

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

const string clientName = "MudExample";
builder.Services.AddScoped<IOllamaApiClient, OllamaApiClient>(sp => new OllamaApiClient("http://localhost:11434/"));
builder.Services.AddSingleton<HttpClientInterceptor>();
builder.Services.AddSingleton(sp =>
        sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient(clientName)
            .EnableIntercept(sp)
    )
    .AddHttpClient(clientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<HttpClientInterceptor>();
builder.Services.AddHttpClientInterceptor();
builder.Services.AddSingleton<MenuViewModel>();
builder.Services.AddSingleton<IMenuService, MenuService>();
builder.Services.AddScoped<IDynamicContentService, DynamicContentService>();
builder.Services.AddScoped<DiaryViewModel>();
builder.Services.AddScoped<IDiaryService, DiaryService>();
builder.Services.AddScoped<INotifyService, NotifyService>();
builder.Services.AddLocalizerAsSingleton();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddSingleton<LayoutState>();
builder.Services.AddLocalization();
builder.Services.AddSubtleCrypto(opt => opt.Key = "kR0BsODSKxPhAWkKpePGmUTvUygYkb9ijbwjnqezc5P8ICszLGyeeVXZJPbaZuTDq8GvP6O4OC92jAse");

builder.Services.AddBlazorDB(options =>
{
    options.Name = "MudExampleDb";
    options.Version = "1";
    options.EncryptionKey = "zQfTuWnZi8u7x!A%C*F-JaBdRlUkXp2l";
    options.StoreSchemas =
        SchemaHelper.GetAllSchemas("MyDatabase"); // builds entire database schema for you based on attributes
});
builder.Services.AddMediaDevicesService();
builder.Services.AddScoped<Selector>();
builder.Services.AddScoped<Selector2>();
builder.Services.AddBrism();


builder.Services.AddMudExtensions();

var app = builder.Build();
await app.UseLocalizer();
using var scope = app.Services.CreateScope();
var vm = scope.ServiceProvider.GetService<MenuViewModel>();
await vm.InitializeAsync();
await app.RunAsync();