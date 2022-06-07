
namespace Postify.Shared.Requests;

public class PostRequest
{

    [Required] public string Title { get; set; } = string.Empty;

    [Required] public string ImageUrl { get; set; } = string.Empty;

    [Required] public string Description { get; set; } = string.Empty;


}
