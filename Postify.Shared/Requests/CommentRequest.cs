
namespace Postify.Shared.Requests;

public class CommentRequest
{
    [Required] public string Body { get; set; } = string.Empty;
}
