using System.ComponentModel.DataAnnotations;

namespace TechStore.Controllers.Models.Users;

public class ApiAuthenticationRequest
{
    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}