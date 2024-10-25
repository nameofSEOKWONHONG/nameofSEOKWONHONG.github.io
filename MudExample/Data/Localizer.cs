using System.Globalization;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using eXtensionSharp;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MudExample.Data;

public static class LocalizerExtensions
{
    public static void AddLocalizerAsSingleton(this IServiceCollection services)
    {
        services.AddSingleton<ILocalizer, Localizer>();
    }

    public static async Task UseLocalizer(this WebAssemblyHost app)
    {
        using var scope = app.Services.CreateScope();
        var localizer = scope.ServiceProvider.GetService<ILocalizer>();
        var localizerInitializer = localizer.xAs<ILocalizerInitilizer>();
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
        
        await localizerInitializer.InitializeAsync(cultureString);
        
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}

public interface ILocalizer
{
    string? this[string type] { get; }
}
public interface ILocalizerInitilizer
{
    Task InitializeAsync(string culture);
}

public class Localizer : ILocalizer, ILocalizerInitilizer
{
    private Dictionary<string, string>? _localizers = new();
    public string? this[string type] 
    {
        get
        {
            _localizers.TryGetValue(type, out var result);
            return result;
        }
    }
    
    private readonly HttpClient _client;
    public Localizer(HttpClient client)
    {
        _client  = client;
    }
    
    public async Task InitializeAsync(string locale)
    {
        var res = await _client.GetAsync($"/languages/{locale.ToLower()}.json?v={Consts.Version}");
        res.EnsureSuccessStatusCode();

        _localizers = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
    }
}