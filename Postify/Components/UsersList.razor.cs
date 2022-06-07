namespace Postify.Components;

public partial class UsersList
{

    [Inject] public UsersService? Users { get; set; }

    [Inject] public NavigationManager? NavManager { get; set; }

    [Inject] public AuthenticationStateProvider? AuthStateProvider { get; set; }

    List<User>? users;

    protected override async Task OnInitializedAsync()
    {
        if (Users is null)
            return;

        if (AuthStateProvider is null)
            return;

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        users = (await Users.GetUsersAsync()).Where(x => x.Id != authState!.User!.FindFirstValue(ClaimTypes.NameIdentifier)).ToList();
    }

    void NavigateToChatRoom(string id)
    {
        if (NavManager is not null)
            NavManager.NavigateTo($"/chat/{id}", true);
    }

}
