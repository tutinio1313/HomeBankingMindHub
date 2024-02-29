using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;

namespace ClientService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController(IClientService clientService) : ControllerBase
{
    [HttpGet]
    public ActionResult<IEnumerable<ClientDTO>> Get()
    {
        IEnumerable<ClientDTO>? clients = clientService.GetAll(out int statusCode, out string? message);

        if (clients is not null)
        {
            return StatusCode(statusCode, clients);
        }
        return StatusCode(statusCode, message);
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
    [HttpGet("current")]
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
    public ActionResult<Client> GetClientByID(string id)
    {
        ClientDTO? client = clientService.GetByID(id, out int statusCode, out string? message);

        if (client is not null)
        {
            return StatusCode(statusCode, client);
        }
        return StatusCode(statusCode, message);
    }
}
