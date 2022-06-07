
namespace Postify;

using Microsoft.AspNetCore.Authentication.JwtBearer;

public class CustomAuthStateProvider : AuthenticationStateProvider
{

    private readonly ProtectedLocalStorage? _localStorage;

    public CustomAuthStateProvider(ProtectedLocalStorage? localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var state = new AuthenticationState(new());

        if(_localStorage is not null)
        {
            var token = await _localStorage.GetAsync<string>("access_token");

            if(!string.IsNullOrEmpty(token.Value))
            {
                var user = new JwtSecurityTokenHandler().ReadJwtToken(token.Value);

                var identity = new ClaimsIdentity(user.Claims, JwtBearerDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                state = new AuthenticationState(principal);

                NotifyAuthenticationStateChanged(Task.FromResult(state));

            }
        }

        return state;
    }
}
