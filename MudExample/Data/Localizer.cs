using System.Globalization;
using System.Net.Http.Json;

namespace MudExample.Data;

public class Localizer
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
        var res = await _client.GetAsync($"/languages/{locale.ToLower()}.json");
        res.EnsureSuccessStatusCode();

        _localizers = await res.Content.ReadFromJsonAsync<Dictionary<string, string>>();
    }
}