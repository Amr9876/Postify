
namespace Postify.Shared;

public record Comment
{
    public string Id { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public string Creator { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
}
