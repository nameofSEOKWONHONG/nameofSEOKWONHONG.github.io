using System.Net.Http.Json;
using Blazored.LocalStorage;
using eXtensionSharp;

namespace MudExample.Services;

public interface INotificationService
{
    Task<List<Notification>> GetNotify();
    Task<bool> SaveNotify(Notification model);
}

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorageService;
    private const string NotifyStorageKey = "notify";
    public NotificationService(HttpClient client, ILocalStorageService localStorageService)
    {
        _httpClient = client;
        _localStorageService = localStorageService;
    }

    public async Task<List<Notification>> GetNotify()
    {
        var res = await _httpClient.GetAsync("data/notification.json");
        res.EnsureSuccessStatusCode();
        
        var result = await res.Content.ReadFromJsonAsync<List<Notification>>();
        await _localStorageService.SetItemAsync(NotifyStorageKey, result);

        return result;
    }

    public async Task<bool> SaveNotify(Notification model)
    {
        var data = await _localStorageService.GetItemAsync<List<Notification>>(NotifyStorageKey);
        var exist = data.FirstOrDefault(m => m.Id == model.Id);
        if (exist.xIsEmpty()) return false;
        
        model.IsRead = true;
        
        await _localStorageService.SetItemAsync(NotifyStorageKey, data);

        return true;
    }
}

public class Notification
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string DisplayName { get; set; }
    public DateTime PublishDate { get; set; }

    public Notification()
    {
    }
}
