
namespace Postify.Shared;

public class ChatMessage
{

    public string UserName { get; set; } = string.Empty;

    public string FromUserId { get; set; } = string.Empty;

    public string ToUserId { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsCurrentUser { get; set; } = false;

    public string Date { get; set; } = DateOnly.FromDateTime(DateTime.Now).ToString();

}
