
namespace Postify.Services;

public class AuthService
{

    private readonly ProtectedLocalStorage _localStorage;

    private readonly HttpClient _http;

    private readonly ILogger<AuthService> _logger;

    public AuthService(ProtectedLocalStorage localStorage, 
                       HttpClient http, 
                       ILogger<AuthService> logger)
    {
        _localStorage = localStorage;
        _http = http;
        _logger = logger;
    }

    public async Task AuthenticateAsync(LoginRequest request)
    {

        var result = await _http.PostAsJsonAsync("authentication/login", request);

        if (result is null)
            return;

        if (result.IsSuccessStatusCode == false)
            return;

        var response = await result.Content.ReadFromJsonAsync<LoginResponse>();

        if (string.IsNullOrEmpty(response?.Token))
            return;

        await _localStorage.SetAsync("access_token", response.Token);

    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        var result = await _http.PostAsJsonAsync("authentication/register", request);

        if(result.IsSuccessStatusCode == false)
        {
            var response = await result.Content.ReadFromJsonAsync<ErrorResponse>();

            _logger.LogError(response!.ErrorMessage);

            return;
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.DeleteAsync("access_token");
    }

}
