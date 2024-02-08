using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController] 
public class ClientController(IClientRepository clientRepository, IPasswordService passwordService) : ControllerBase
{
#pragma warning disable
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IPasswordService passwordService = passwordService;
#pragma warning restore

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

                foreach (Client client in clients)
                {
                    clientDTOs[index] = new()
                    {
                        ID = index.ToString(),
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,

                        Accounts = client.Accounts.Select(account => new AccountDTO
                        {
                            ID = account.Id,
                            Number = account.Number,
                            CreationDate = account.CreationTime,
                            Balance = account.Balance
                        }).ToArray(),

                        
                        Credits = client.Loans.Select(loan => new ClientsLoanDTO
                        {
                            ID = loan.ID,
                            LoanID = loan.LoanID,
                            Name = loan.Loan.Name,
                            Amount = loan.Amount,
                            Payments = loan.Payment
                        }).ToArray(),

                        Cards = client.Cards.Select(card => new CardDTO
                        {
                            Id = card.Id,
                            CardHolder = card.CardHolder,
                            Type = card.Type.ToString(),
                            Color = card.Color.ToString(),
                            Number = card.Number,
                            CVV = card.CVV,
                            FromDate = card.FromDate,
                            ThruDate = card.ThruDate
                        }).ToArray()
                    };

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
    [HttpGet("current")]
    public IActionResult GetCurrent()
    {
        string? Email = User.FindFirst("Client")?.Value;

        if (Email is not null)
        {
            Client? client = _clientRepository.FindByEmail(Email);

            if(client is not null) 
            {
                return Ok(new ClientDTO
            {

                ID = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,

                Accounts = client.Accounts.Select(account => new AccountDTO
                {
                    ID = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationTime,
                    Balance = account.Balance
                }).ToArray(),

                Credits = client.Loans.Select(loan => new ClientsLoanDTO
                {
                    ID = loan.ID,
                    LoanID = loan.LoanID,
                    Name = loan.Loan.Name,
                    Amount = loan.Amount,
                    Payments = loan.Payment
                }).ToArray(),

                Cards = client.Cards.Select(card => new CardDTO
                {
                    Id = card.Id,
                    CardHolder = card.CardHolder,
                    Type = card.Type.ToString(),
                    Color = card.Color.ToString(),
                    Number = card.Number,
                    CVV = card.CVV,
                    FromDate = card.FromDate,
                    ThruDate = card.ThruDate
                }).ToArray()
            });
            } 
            else
            {
                return Forbid();
            }
        }

        else
        {
            return Forbid();
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Client> Get(string id)
    {
        Client? client = _clientRepository.FindByID(id);

        if (client is not null)
        {
            int index = 0;
            return Ok(new ClientDTO
            {

                ID = index.ToString(),
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,

                Accounts = client.Accounts.Select(account => new AccountDTO
                {
                    ID = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationTime,
                    Balance = account.Balance
                }).ToArray(),

                Credits = client.Loans.Select(loan => new ClientsLoanDTO
                {
                    ID = loan.ID,
                    LoanID = loan.LoanID,
                    Name = loan.Loan.Name,
                    Amount = loan.Amount,
                    Payments = loan.Payment
                }).ToArray(),

                Cards = client.Cards.Select(card => new CardDTO
                {
                    Id = card.Id,
                    CardHolder = card.CardHolder,
                    Type = card.Type.ToString(),
                    Color = card.Color.ToString(),
                    Number = card.Number,
                    CVV = card.CVV,
                    FromDate = card.FromDate,
                    ThruDate = card.ThruDate
                }).ToArray()
            });
        }
        return Ok("No se ha encontrado el cliente.");
    }

    [HttpPost]
    public IActionResult Post([FromBody] PostModel model)
    {
        bool userEmailExists = _clientRepository.FindByEmail(model.Email) is not null;

        if (!userEmailExists)
        {
            try
            {
                int result = _clientRepository.Save(new()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = passwordService.HashPassword(model.Password)
                });

                //The result means the entity amount changes on DB, that's the reason about the following condition.

                if (result >= 1)
                {
                    return Created();

                }
                else
                {
                    return Ok("Algo ha salido mal creando el usuario.");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        else
        {
            return Ok("El usuario ya esta cargado!");
        }
    }
    /*

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }*/
}
