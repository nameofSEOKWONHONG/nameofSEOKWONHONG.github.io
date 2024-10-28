using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace MudExample.Data;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string RoleId { get; set; }
    public Role Role { get; set; }
}

public class Role
{
    public string Id { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
    public string[] UserRoles { get; set; }
}

public interface IUserService
{
    
}

public class UserService
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private const string UserStorageKey = "user";
    private const string RoleStorageKey = "role";

    public UserService(HttpClient client, ILocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
    }

    public async Task<List<User>> GetUsers()
    {
        var response = await _client.GetAsync("data/users");
        response.EnsureSuccessStatusCode();

        var users = await response.Content.ReadFromJsonAsync<List<User>>();
        
        return users;
    }

    public async Task<List<Role>> GetRoles()
    {
        var response = await _client.GetAsync("data/roles");
        response.EnsureSuccessStatusCode();

        var roles = await response.Content.ReadFromJsonAsync<List<Role>>();
        
        return roles;
    }
}