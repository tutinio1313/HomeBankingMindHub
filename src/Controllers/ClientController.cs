using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController(IAccountService accountService, IClientService clientService, ICardService cardService) : ControllerBase
{
#pragma warning disable

#pragma warning restore

    [HttpGet]
    public ActionResult<IEnumerable<ClientDTO>> Get()
    {
        IEnumerable<ClientDTO>? clients = clientService.GetAll(out int statusCode, out string? message);

        if (clients is not null)
        {
            return StatusCode(statusCode, message);
        }
        return StatusCode(statusCode, message);
    }
    [HttpGet("current")]
    [Authorize]
    public IActionResult GetCurrent()
    {
        ClientDTO? client = clientService.GetCurrent(User, out int statusCode, out string? message);

        if (client is not null)
        {
            return StatusCode(statusCode, client);
        }

        return StatusCode(statusCode, message);
    }

    [HttpGet("{id}")]
    public ActionResult<Client> Get(string id)
    {
        ClientDTO? client = clientService.GetByID(id, out int statusCode, out string? message);

        if (client is not null)
        {
            return StatusCode(statusCode, client);
        }
        return StatusCode(statusCode, message);
    }
    [HttpGet("current/account")]
    [Authorize]
    public IActionResult GetCurrentAccounts()
    {
        string? UserEmail = User.FindFirstValue(ClaimTypes.Email);

        if (!UserEmail.IsNullOrEmpty())
        {
            #pragma warning disable
            IEnumerable<AccountDTO>? accountsDTOs = accountService.GetAllAcountsByEmail(UserEmail: UserEmail, out int statusCode, out string? message);
            #pragma warning restore
            if (accountsDTOs is not null)
            {
                return StatusCode(statusCode, accountsDTOs);
            }

            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }
    [HttpPost]
    public IActionResult Post([FromBody] PostModel model)
    {
        ClientDTO? client = clientService.CreateUser(model, out int statusCode, out string? message);

        if (client is not null)
        {
            return StatusCode(statusCode, client);
        }

        return StatusCode(statusCode, message);
    }
    [HttpPost("current/accounts")]
    [Authorize]
    public ActionResult PostAccount()
    {
        string? UserEmail = User.FindFirstValue(ClaimTypes.Email);
#pragma warning disable
        if (!UserEmail.IsNullOrEmpty())
        {
            AccountDTO? response = accountService.PostAccount(UserEmail
            , out int statusCode
            , out string? message);
#pragma warning restore
            if (statusCode == 201)
            {
                return StatusCode(201, response);
            }

            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }

    [HttpPost("current/cards")]
    [Authorize]
    public ActionResult PostCard(PostCardModel model)
    {
        string? UserEmail = User.FindFirstValue(ClaimTypes.Email);

        if (!UserEmail.IsNullOrEmpty())
        {
#pragma warning disable
            CardDTO? card = cardService.PostCard(model: model, Email: UserEmail, out int statusCode, out string? message);
#pragma warning restore
            if (card is not null)
            {
                return StatusCode(statusCode, card);
            }
            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }

    [HttpGet("current/cards")]
    [Authorize]
    public ActionResult GetCards()
    {

        string? UserEmail = User.FindFirstValue(ClaimTypes.Email);

        if (!UserEmail.IsNullOrEmpty())
        {
            #pragma warning disable
            CardDTO[]? cardDTOs = cardService.GetDTOCards(UserEmail, out int statusCode, out string? message);
            #pragma warning restore
            if (cardDTOs is not null)
            {
                return StatusCode(statusCode, cardDTOs);
            }
            return StatusCode(statusCode, message);
        }

        return StatusCode(401, "Usted no tiene los permisos necesarios.");
    }

}
