using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ClientRepository clientRepository) : ControllerBase
{
    private readonly IClientRepository clientRepository = clientRepository;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Client model)
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

    [HttpGet("{id}")]
    public ActionResult<string> Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
