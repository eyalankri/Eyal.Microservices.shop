using System.ComponentModel.DataAnnotations;
namespace Members;


public record UserDto(Guid Id,string Password ,string Email,string Role ,string Surname,string Name,DateTimeOffset DateCreated);

public record CreateUserDto([Required] string Email,[Required] string Password,[Required] string Name,[Required] string Surname,[Required] string Role = "Registered");

public record LoginUserDto([Required] string Email, [Required] string Password);


