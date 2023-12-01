using Members.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Members.Controllers;


[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("Public")]
    public IActionResult Public()
    {
        return Ok("Hi, you're on public property");
    }


    [HttpGet("Admins")]
    [Authorize(Roles = "Administrator")]
    public IActionResult AdminsEndpoint()
    {
        var currentUser = GetCurrentUser();
        if (currentUser != null)
        {
            return Ok($"Hi {currentUser.Name}, you are an {currentUser.Role}");
        }
        return BadRequest();
    }


    [HttpGet("")]
    [Authorize(Roles = "Administrator,Seller")]
    public IActionResult SellersEndpoint()
    {
        var currentUser = GetCurrentUser();
        if (currentUser != null)
        {
            return Ok($"Hi {currentUser.Name}, you are a {currentUser.Role}");
        }
        return BadRequest();
    }



    private User? GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            var userClaims = identity.Claims;

            return new User
            {
                //Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value!,
                Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value!,
                Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value!,
                Surname = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Surname)?.Value!,
                Role = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value!
            };
        }
        return null;
    }
}