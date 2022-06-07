
namespace Postify.Shared.Requests;

public class LoginRequest : EmailRequest
{
    [Required]
    public string Password { get; set; } = string.Empty;
}
