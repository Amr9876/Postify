
namespace Postify.Components;

public partial class ChatScreen : IAsyncDisposable
{

    [Parameter] public string Id { get; set; } = string.Empty;

    [Inject] public UsersService? Users { get; set; }

    [Inject] public NavigationManager? NavManager { get; set; }

    [Inject] public IConfiguration? Config { get; set; }

    [Inject] public AuthenticationStateProvider? AuthStateProvider { get; set; }

    [Inject] public ProtectedLocalStorage? LocalStorage { get; set; }


    HubConnection? hubConnection;

    User? user;

    List<ChatMessage>? messages;

    string inputMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {

        if (Users is null ||
            string.IsNullOrEmpty(Id) ||
            NavManager is null ||
            AuthStateProvider is null ||
            LocalStorage is null ||
            Config is null)
            return;

        user = await Users.GetUserAsync(Id);

        messages = new();

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        var token = await LocalStorage.GetAsync<string>("access_token");

        var connectionOptions = (HttpConnectionOptions options) =>
        {
            options.AccessTokenProvider = () => Task.FromResult(token.Value);
        };


        hubConnection = new HubConnectionBuilder()
                            .WithUrl(Config["HubAddress"], connectionOptions)
                            .Build();

        hubConnection.On<ChatMessage>("RecievePrivateMessage", async (message) =>
        {
            var chatMessage = new ChatMessage
            {
                FromUserId = message.FromUserId,
                ToUserId = message.ToUserId,
                UserName = message.UserName,
                Content = message.Content,
                IsCurrentUser = message.UserName == authState!.User!.FindFirstValue("FirstName"),
                Date = message.Date
            };

            messages.Add(chatMessage);

            await InvokeAsync(StateHasChanged);
        });

        await hubConnection.StartAsync();
    }

    async Task SendAsync()
    {

        if (AuthStateProvider is null)
            return;

        if (string.IsNullOrEmpty(inputMessage))
            return;

        if (hubConnection is null)
            return;

        var authState = await AuthStateProvider.GetAuthenticationStateAsync();

        var chatMessage = new ChatMessage
        {
            ToUserId = Id,
            FromUserId = authState!.User!.FindFirstValue(ClaimTypes.NameIdentifier),
            UserName = authState!.User!.FindFirstValue("FirstName"),
            Content = inputMessage,
            Date = DateOnly.FromDateTime(DateTime.Now).ToString(),
        };

        await hubConnection.SendAsync("SendPrivateMessage", chatMessage);

        inputMessage = "";

    }

    bool isConnected => hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
            await hubConnection.DisposeAsync();
    }

}
