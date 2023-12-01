using System.ComponentModel.DataAnnotations;
namespace Members.Entities;

public record CreateUserDto
(
    [Required] string Email, 
    [Required] string Password, 
    [Required] string Name, 
    [Required] string Surname,
    [Required] string Role = "Registered"
);


public record LoginUserDto([Required] string Email,[Required] string Password);
     

