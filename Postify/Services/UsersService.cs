namespace Postify.Services;

public class UsersService
{

    private readonly HttpClient _http;

    public UsersService(HttpClient http,
                        ILogger<UsersService> logger)
    {
        _http = http;
    }

    public async Task<List<User>> GetUsersAsync()
    {

        var result = await _http.GetFromJsonAsync<UsersResponse>("users");

        if (result is null)
            return new();

        return result.Users;
    }

    public async Task<User> GetUserAsync(string? id)
    {

        var result = await _http.GetFromJsonAsync<UserResponse>($"users/{id}");

        if (result is null)
            return new();

        return result.User;

    }

}
