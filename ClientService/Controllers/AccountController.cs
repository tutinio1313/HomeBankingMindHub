using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace ClientService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<AccountDTO>> Get()
    {
        var accounts = accountService.GetAll(
            out int statusCode,
             out string? message);

        if (accounts is not null)
        {
            return StatusCode(statusCode, accounts);
        }

        return StatusCode(statusCode, message);
    }
    [HttpGet("{id}")]
    public ActionResult<AccountDTO> Get(string id)
    {
        var account = accountService.GetByID(id: id,
         out int statusCode,
         out string? message);

        if (account is not null)
        {
            return StatusCode(statusCode, account);
        }

        return StatusCode(statusCode, message);
    }
    [HttpGet("current/account")]
    public IActionResult GetCurrentAccounts()
    {
        string? UserEmail = User.FindFirstValue("Client");

        if (!UserEmail.IsNullOrEmpty())
        {
            IEnumerable<AccountDTO>? accountsDTOs = accountService.GetAllAcountsByEmail(UserEmail: UserEmail, out int statusCode, out string? message);
            if (accountsDTOs is not null)
            {
                return StatusCode(statusCode, accountsDTOs);
            }

            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }
    [HttpPost("current/accounts")]
    public ActionResult PostAccount([FromBody] string email)
    {
        if (!email.IsNullOrEmpty())
        {
            AccountDTO? response = accountService.PostAccount(email
            , out int statusCode
            , out string? message);

            if (statusCode == 201)
            {
                return StatusCode(201, response);
            }

            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }
}
