using System.Net.Http.Json;
using Blazored.LocalStorage;
using eXtensionSharp;

namespace MudExample.Services;

public interface INotifyService
{
    Task<List<Notify>> GetNotify();
    Task<bool> SaveNotify(Notify model);
}

public class NotifyService : INotifyService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private const string NotifyStorageKey = "notify";
    public NotifyService(HttpClient client, ILocalStorageService localStorageService)
    {
        _httpClient = client;
        _localStorageService = localStorageService;
    }

    public async Task<List<Notify>> GetNotify()
    {
        var res = await _httpClient.GetAsync("data/notification.json");
        res.EnsureSuccessStatusCode();
        
        var result = await res.Content.ReadFromJsonAsync<List<Notify>>();
        await _localStorageService.SetItemAsync(NotifyStorageKey, result);

        return result;
    }

    public async Task<bool> SaveNotify(Notify model)
    {
        var data = await _localStorageService.GetItemAsync<List<Notify>>(NotifyStorageKey);
        var exist = data.FirstOrDefault(m => m.Id == model.Id);
        if (exist.xIsEmpty()) return false;
        
        model.IsRead = true;
        
        await _localStorageService.SetItemAsync(NotifyStorageKey, data);

        return true;
    }
}

public class Notify
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string DisplayName { get; set; }
    public DateTime PublishDate { get; set; }

    public Notify()
    {
    }
}
