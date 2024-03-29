using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

using HomeBankingMindHub.Model.Model.Auth;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        ClaimsIdentity? result = authService.Login(model, out int statusCode, out string? message);

        if(result is not null)
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(result));
            return StatusCode(statusCode);
        }

        return StatusCode(statusCode, message);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Post()
    {
        try {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        catch (Exception ex) {
            return StatusCode(500, ex);
        }
    }
}
