using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;

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
        
        if(clients is not null)
        {
            return StatusCode(statusCode, message);
        }
        return StatusCode(statusCode, message);
    }
    [HttpGet("current")]
    public IActionResult GetCurrent()
    {
        ClientDTO? client = clientService.GetCurrent(User, out int statusCode, out string? message);
        
        if(client is not null)
        {
            return StatusCode(statusCode, client);
        }

        return StatusCode(statusCode, message);
    }   

    [HttpGet("{id}")]
    public ActionResult<Client> Get(string id)
    {
        ClientDTO? client = clientService.GetByID(id, out int statusCode, out string? message);
        
        if(client is not null) {
            return StatusCode(statusCode, client);
        }
        return StatusCode(statusCode, message);
    }

    [HttpPost]
    public IActionResult Post([FromBody] PostModel model)
    {
        ClientDTO? client = clientService.CreateUser(model, out int statusCode, out string? message);

        if(client is not null)
        {
            return StatusCode(statusCode, client);
        }

        return StatusCode(statusCode, message);
    }
    [HttpPost("current/accounts")]
    public ActionResult PostAccount()
    {
        AccountDTO? response = accountService.PostAccount(User
        ,out int statusCode
        , out string? message);

        if(statusCode == 201){
            return StatusCode(201, response);
        }

        return StatusCode(statusCode, message);
    }

    [HttpPost("current/cards")]

    public ActionResult PostCard(PostCardModel model)
    {
        CardDTO? card = cardService.PostCard(model: model, claims: User, out int statusCode, out string? message);

        if(card is not null)
        {
            return StatusCode(statusCode, card);
        }

        return StatusCode(statusCode, message);
    }
}
