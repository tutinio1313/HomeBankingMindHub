using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Utils;

using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.Serialization;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;

namespace HomeBankingMindHub.Service.Instance;

public class ClientService(IClientRepository _clientRepository, IAccountRepository _accountRepository) : IClientService
{
    public ClientDTO? CreateUser(PostModel model, out int StatusCode, out string? message)
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
                    Password = HomeBankingMindHub.Utils.Utils.HashPassword(model.Password)
                };

                int result = _clientRepository.Save(user);

                _accountRepository.Save(new Account
                {
                    Id = Guid.NewGuid().ToString(),
                    CreationTime = DateTime.Now,

                    Client = user,
                    ClientId = user.Id,

                    Number = Utils.Utils.GenerateAccountNumber(random: new(), _accountRepository: ref _accountRepository)
                });

                //The result means the entity amount changes on DB, that's the reason about the following condition.

                if (result >= 1)
                {
                    StatusCode = 201;
                    message = "¡El usuario {user.FirstName} {user.LastName}, se ha creado correctamente!";
                    return new ClientDTO(){
                        ID = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    };
                }
                else
                {
                    StatusCode = 400;
                    message = "No se pudo cargar el usuario.";
                }

            }
            catch (Exception ex)
            {
                StatusCode = 500;
                message = ex.ToString();
            }
        }
        else
        {
            StatusCode = 200;
            message = "El usuario ya esta cargado!";
        }
        return null;
    }

    public IEnumerable<ClientDTO>? GetAll(out int StatusCode, out string? message)
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
                        #pragma warning disable

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
                        #pragma warning restore
                    };

                    index++;
                }

                StatusCode= 200;
                message = null;
                return clientDTOs;
            }

            catch
            {
                StatusCode = 500;
                message = "Algo ha salido mal procesado tu pedido.";
            }
        }
        else
        {
            StatusCode = 200;
            message = "No hay clientes cargados.";
        }

        return null;
    }

    public ClientDTO? GetByID(string id, out int StatusCode, out string? message)
    {
            Client? client = _clientRepository.FindByID(id);

        if (client is not null)
        {
            int index = 0;
            
            message = null;
            StatusCode = 200;

            return new ClientDTO
            {

                ID = index.ToString(),
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                #pragma warning disable
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
                #pragma warning restore
            };
        }
        StatusCode = 200;
        message = "No se ha encontrado el cliente.";

        return null;
    }

    public ClientDTO? GetCurrent(ClaimsPrincipal claims,out int StatusCode, out string? message)
    {
        string? Email = claims.FindFirst("Client")?.Value;

        if (Email is not null)
        {
            Client? client = _clientRepository.FindByEmail(Email);

            if (client is not null)
            {
                StatusCode = 200;
                message = null;
                return new ClientDTO
                    {

                        ID = client.Id,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,
                        #pragma warning disable

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
                        #pragma warning restore
                    };
            }
            else
            {
                StatusCode = 403;
                message = "No se ha podido identificar tu usuario.";
            }
        }

        else
        {
            StatusCode = 403;
            message = "El email ingresado no es correcto.";
        }

        return null;
    }
}