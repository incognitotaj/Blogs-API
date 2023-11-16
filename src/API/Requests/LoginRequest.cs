using System.ComponentModel.DataAnnotations;

namespace API.Requests;

public class LoginRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
