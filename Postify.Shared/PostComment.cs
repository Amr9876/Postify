
namespace Postify.Shared;

public record PostComment
{
    public string PostId { get; set; } = string.Empty;

    public string CommentId { get; set; } = string.Empty;
}
