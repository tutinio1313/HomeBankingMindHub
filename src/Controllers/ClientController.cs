using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using HomeBankingMindHub.Service.Interface;
using System.Security.Claims;
using System;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClientController(IClientRepository clientRepository, IPasswordService passwordService, IAccountRepository _accountRepository, ICardRepository _cardRepository) : ControllerBase
{
#pragma warning disable
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IPasswordService passwordService = passwordService;
    private readonly IAccountRepository _accountRepository = _accountRepository;
    private readonly ICardRepository _cardRepository = _cardRepository;
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

            if (client is not null)
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
                Client user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = passwordService.HashPassword(model.Password)
                };

                int result = _clientRepository.Save(user);

                _accountRepository.Save(new Account{ 
                    Id = Guid.NewGuid().ToString(),
                    CreationTime = DateTime.Now,

                    Client = user,
                    ClientGuid = user.Id,

                    Number = GenerateAccountNumber(new())
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
    [HttpPost("current/accounts")]
    public ActionResult PostAccount()
    {
        string Id = User.FindFirstValue("Client");

        if (Id is not null)
        {
            try
            {
                bool canPost = _accountRepository.GetAccountsByClient(Id).Count() < 3;
                Client? user = _clientRepository.FindByEmail(Id);


                if (canPost && user is not null)
                {
                    Random random = new();
                    string AccountNumber = GenerateAccountNumber(random);
                    Account account = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreationTime = DateTime.Now,
                        Balance = 0,
                        Number = AccountNumber,

                        ClientGuid = user.Id,
                        Client = user,
                    };

                    _accountRepository.Save(account);
                    return Created();
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        else
        {
            return Unauthorized();
        }

    }
    private string GenerateAccountNumber(Random random)
    {
        string accountNumber;
        do
        {
            accountNumber = "VIN-" + random.Next(1, 100000000).ToString();
        } while (_accountRepository.ExistsAccountByNumber(accountNumber));
        return accountNumber;
    }

    [HttpPost("current/cards")]

    public ActionResult PostCard(PostCardModel model)
    {

        string Id = User.FindFirstValue("Client");

        if (Id is not null)
        {
            try
            {
                Client? client = _clientRepository.FindByEmail(Id);
                Random random = new();

                if (ValidateCardModel(model, out CardColor? CardColor, out CardType? CardType) && client is not null)
                {
                    client.Cards.Add(new Card
                    {
                        Id = Guid.NewGuid().ToString(),
                        CardHolder = string.Concat(string.Concat(client.FirstName, " "), client.LastName),
                        client = client,
                        ClientID = client.Id,

                        Color = (CardColor)CardColor,
                        Type = (CardType)CardType,

                        CVV = random.Next(1, 1000),
                        Number = GenerateCardNumber(random),

                        FromDate = DateTime.Now.AddMonths(-1),
                        ThruDate = DateTime.Now.AddYears(2)

                    });

                    _clientRepository.Put(client);

                    return Created();
                }

                return Forbid();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        else
        {
            return Unauthorized();
        }
    }

    private static bool ValidateCardModel(PostCardModel model, out CardColor? cardColor, out CardType? cardType)
    {

        bool boolColor = ValidateCardColor(model.Color, out cardColor);
        bool boolType = ValidateCardType(model.Type, out cardType);

        return boolColor && boolType;
    }

    private string GenerateCardNumber(Random random)
    {
        string cardNumber = string.Empty;
        do
        {
            cardNumber = random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString() + "-" + random.Next(1000, 10000).ToString();
        } while (_cardRepository.ExistsCardByNumber(cardNumber));
        return cardNumber;
    }
           
    private static bool ValidateCardColor(string color, out CardColor? cardColor)
    {
        switch (color.ToUpper())
        {
            case nameof(CardColor.GOLD):
                cardColor = CardColor.GOLD;
                return true;

            case nameof(CardColor.TITANIUM):
                cardColor = CardColor.TITANIUM;
                return true;

            case nameof(CardColor.SILVER):
                cardColor = CardColor.SILVER;
                return true;
            default:
                cardColor = null;
                return false;
        }
    }
    private static bool ValidateCardType(string type, out CardType? cardType)
    {
        if (type == CardType.CREDIT.ToString())
        {
            cardType = CardType.CREDIT;
            return true;
        }
        if (type == CardType.DEBIT.ToString())
        {
            cardType = CardType.DEBIT;
            return true;
        }

        cardType = null;
        return false;

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
