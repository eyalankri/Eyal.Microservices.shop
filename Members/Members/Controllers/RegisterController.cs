using Common.Repositories;
using Members.Constants;
using Members.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Members.Controllers;


[Route("[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{

    private readonly IConfiguration _config;
    private readonly IRepository<User> _userRepository;
    private readonly string? _salt;


    public RegisterController(IConfiguration config, IRepository<User> userRepository)
    {
        _userRepository = userRepository;
        _config = config;
        _salt = _config["MembersApi:PasswordSalt"]; //secret
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserDto createUserDto)
    {
        if (!Roles.List.Contains(createUserDto.Role))
        {
            return BadRequest("Not a valid role");
        }
        
        var user = await _userRepository.GetAsync(u => u.Email == createUserDto.Email);

        if (user != null)
        {
            return NotFound("Email is already exists");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, _salt);

        var newUser = new User
        {
            Email = createUserDto.Email,
            Password = passwordHash,
            Name = createUserDto.Name,
            Surname = createUserDto.Surname,
            Role = createUserDto.Role
        };

        await _userRepository.CreateAsync(newUser);
        
        return Ok("User created");
    }
}
