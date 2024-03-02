using System.ComponentModel.DataAnnotations;

namespace Requiem.PetProject.DTOs.Account;

public class LoginDto
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}