using System.Net.Http.Json;

namespace MudExample.Components;

public class SelectItem
{
    public string Text { get; set; }
    public string Value { get; set; }
    
    
}

public interface ISelector
{
    Task<List<SelectItem>> GetItems();
}

public class Selector : ISelector
{
    private readonly HttpClient _httpClient;
    public Selector(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<SelectItem>> GetItems()
    {
        var res = await _httpClient.GetAsync("/data/select-data.json");
        res.EnsureSuccessStatusCode();

        var items = await res.Content.ReadFromJsonAsync<List<SelectItem>>();
        await Task.Delay(200);

        return items;
    }
}

public class Selector2 : ISelector
{
    private readonly HttpClient _httpClient;
    public Selector2(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<SelectItem>> GetItems()
    {
        var res = await _httpClient.GetAsync("/data/select-data2.json");
        res.EnsureSuccessStatusCode();

        var items = await res.Content.ReadFromJsonAsync<List<SelectItem>>();
        await Task.Delay(200);

        return items;
    }
}