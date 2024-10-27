using System.Net.Http.Json;
using eXtensionSharp;
using MudBlazor;
using MudExample.Data;

namespace MudExample.Services;

public interface IDynamicContentService
{
    Task<string> GetDynamicContent(string name);
    Task<string> GetSelectBox(string name);
}

public class DynamicContentService: IDynamicContentService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalizer _localizer;
    public DynamicContentService(HttpClient httpClient, ILocalizer localizer)
    {
        _httpClient = httpClient;
        _localizer = localizer;
    }

    public async Task<string> GetDynamicContent(string name)
    {
        var res = await _httpClient.GetAsync($"/data/dynamicContent.json");
        res.EnsureSuccessStatusCode();
        
        var result = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return result[name];
    }

    public async Task<string> GetSelectBox(string name)
    {
        var res = await _httpClient.GetAsync($"/data/dynamicContent.json");
        res.EnsureSuccessStatusCode();
        
        var result = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return result[name];
    }
}