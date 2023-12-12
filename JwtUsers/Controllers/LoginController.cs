using Common.Repositories;
using Common.Settings;
using Members.Entities;
using Members.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Members.Controllers
{
    [Route("[controller]")]  
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IRepository<User> _userRepository;
        private readonly MembersApiSettings _membersApiSettings;
        private readonly JwtSettings _jwtSettings;


        public LoginController(IConfiguration config, IRepository<User> userRepository)
        {
            _config = config;

            _membersApiSettings = _config.GetSection(nameof(MembersApiSettings)).Get<MembersApiSettings>()!;
            _jwtSettings = _config.GetSection(nameof(JwtSettings)).Get<JwtSettings>()!;


            if (_membersApiSettings == null || _jwtSettings == null)
            {
                throw new NullReferenceException("Missing settings");

            }
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [EnableCors]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            var user = await Authenticate(loginUserDto);

            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               // new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
              _jwtSettings.Issuer,
              _jwtSettings.Audience,
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<User?> Authenticate(LoginUserDto loginUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(loginUserDto.Password, _membersApiSettings.PasswordSalt);

            User user = await _userRepository.GetAsync(x => x.Email.ToLower() == loginUserDto.Email.ToLower() && x.Password == passwordHash);

            if (user == null)
            {
                return null;
            }
            return user;
        }


    }
}