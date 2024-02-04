using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.DTO;
using System.Collections.Immutable;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController(IClientRepository clientRepository) : ControllerBase
{
    private readonly IClientRepository _clientRepository = clientRepository;

    [HttpGet]
    public ActionResult<IEnumerable<Client>> Get()
    {
        var clients = _clientRepository.GetAllUsers();

        if (clients is not null)
        {
            try
            {
                ClientDTO[] clientDTOs = new ClientDTO[clients.Count()];
                int index = 0;
                int accountIndex = 0;

                foreach (Client client in clients)
                {
                    clientDTOs[index] = new()
                    {
                        ID = index,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,

                        Accounts = client.Accounts.Select(account => new AccountDTO
                        {
                            ID = accountIndex++,
                            Number = account.Number,
                            CreationTime = account.CreationTime,
                            Balance = account.Balance
                        }).ToArray()
                    };
                    accountIndex = 0;
                    index++;
                }

                return Ok(clientDTOs);
            }

            catch
            {
                return StatusCode(500, "Algo ha salido mal procesado tu pedido.");
            }
        }

        return Ok("No hay clientes cargados.");
    }

    [HttpGet("{id}")]
    public ActionResult<Client> Get(string id)
    {
        Client? client = _clientRepository.FindByID(id);

        if (client is not null)
        {
            int index = 0;
            int accountIndex = 0;
            return Ok(new ClientDTO
            {

                ID = index,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,

                Accounts = client.Accounts.Select(account => new AccountDTO
                {
                    ID = accountIndex++,
                    Number = account.Number,
                    CreationTime = account.CreationTime,
                    Balance = account.Balance
                }).ToArray()

            });
        }
        return Ok("No se ha encontrado el cliente.");
    }

    /*
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
    }*/
}
