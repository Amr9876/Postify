
namespace Postify.Components;

public partial class Form
{

    [Parameter] public FormType FormType { get; set; } = FormType.None;

    [Inject] public NavigationManager? NavManager { get; set; }

    [Inject] public AuthService? AuthenticationService { get; set; }

    private LoginRequest loginRequest = new();

    private RegisterRequest registerRequest = new();

    private string errorMessage = string.Empty;

    private async Task LoginHandler()
    {

        try
        {
            
            if(AuthenticationService is not null)
            {
                await AuthenticationService.AuthenticateAsync(loginRequest);
                
                if(NavManager is not null)
                    NavManager.NavigateTo("/", true);
            }

        }
        catch (Exception e)
        {
            errorMessage = e.Message;
        }

    }

    private async Task RegisterHandler()
    {

        if(registerRequest is not null)
        {

            try
            {
   
                if(AuthenticationService is not null &&
                   NavManager is not null) 
                   {
                        await AuthenticationService.RegisterAsync(registerRequest);
                        NavManager.NavigateTo("/login");
                   }  

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

        }

    }

}