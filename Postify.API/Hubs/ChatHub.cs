namespace Postify.API.Hubs;

public class ChatHub : Hub
{

    public async Task SendPrivateMessage(ChatMessage message)
    {
        await Clients.Users(message.FromUserId, message.ToUserId).SendAsync("RecievePrivateMessage", message);
    }

}
