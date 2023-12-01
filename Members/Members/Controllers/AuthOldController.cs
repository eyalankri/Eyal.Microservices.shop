using Members.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Members.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthOldController : ControllerBase
    {
        private static User _user = new User();
        private string? salt;
        private readonly IConfiguration config;


        public AuthOldController(IConfiguration config)
        {
            this.config = config;
            this.salt = config["MembersApi:PasswordSalt"]; //secret
        }

        [HttpPost("register")]
        public ActionResult<User> Register(CreateUserDto createUserDto)
        {
            if (salt == null)
            {
                return BadRequest();
            }
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, salt);

            _user.Email = createUserDto.Email;
            //_user.PasswordHash = passwordHash;

            return Ok(_user);

        }

        [HttpPost("login")]
        public ActionResult<User> Login(CreateUserDto createUserDto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, salt);

            //if (_user.PasswordHash != passwordHash || _user.Username != request.Email)
            //{
            //    return BadRequest("The credentials are incorrect");
            //}

            string token = CreateToken(_user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name,user.Username)
            };

            var securityKey = config["MembersApi:JwtToken"];
            var key = new SymmetricSecurityKey(Convert.FromBase64String(securityKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
            
            // implemented on WheaterForcastController
        }
    }
}
