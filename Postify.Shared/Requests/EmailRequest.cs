global using System.ComponentModel.DataAnnotations;

namespace Postify.Shared.Requests;

public class EmailRequest 
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
}
