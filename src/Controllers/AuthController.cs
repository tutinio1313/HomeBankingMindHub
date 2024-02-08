using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IClientRepository clientRepository) : ControllerBase
{
    private readonly IClientRepository clientRepository = clientRepository  ;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try{
            Client? user = clientRepository.FindByEmail(model.Email);

            if (user is not null)
            {
                if (model.Password.Equals(user.Password))
                {
                    ClaimsIdentity claimsIdentity = new(claims: [new Claim("Client", user.Email)], authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        catch (Exception ex){
            return StatusCode(500,ex);
        }
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
