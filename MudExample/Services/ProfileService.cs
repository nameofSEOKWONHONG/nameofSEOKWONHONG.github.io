namespace MudExample.Services;

public interface IProfileService
{
    Profile GetProfile();
}

public class ProfileService : IProfileService
{
    public Profile GetProfile()
    {
        return new Profile("tester", "mr", new DateTime(1970, 1, 1), "https://avatars.githubusercontent.com/u/24273022?v=4", true);
    }
}

public record Profile(string FirstName, string LastName, DateTime Birthday, string ImagePath, bool IsLogin)
{
    public string FullName => $"{FirstName} {LastName}";
    public bool IsLogin { get; set; } = IsLogin;
}