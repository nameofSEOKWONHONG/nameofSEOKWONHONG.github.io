using System.Net.Http.Json;
using Blazored.LocalStorage;
using eXtensionSharp;

namespace MudExample.Data;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public int RoleId
    {
        get
        {
            if (this.Role.xIsNotEmpty())
                return this.Role.Id;

            return 0;
        }
    }

    public string RoleName
    {
        get
        {
            if (this.Role.xIsNotEmpty())
                return this.Role.RoleName;
            
            return string.Empty;
        }
    }
    
    public DateTime? WeatherDate { get; set; }
    public string WeatherSummary { get; set; }

    public Role Role { get; set; }

    public User()
    {
        
    }

    public User(int id, string name, string email, Role role)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
        this.Role = role;
    }
}

public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public string RoleDescription { get; set; }
    public string[] UserRoles { get; set; }

    public Role()
    {
        
    }

    public Role(int id, string roleName, string roleDescription, string[] userRoles)
    {
        this.Id = id;
        this.RoleName = roleName;
        this.RoleDescription = roleDescription;
        this.UserRoles = userRoles;
    }

    public static List<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role(1, "Administrator", "Administrator", null),
            new Role(2, "User", "User", null),
            new Role(3, "Guest", "Guest", null),
        };
    }
    
    public static Func<Role, string> ConvertRoleToRoleName = ci => ci?.RoleName;
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