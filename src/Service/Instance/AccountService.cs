using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Identity.Client;

using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Database.Repository;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Collections.ObjectModel;

namespace HomeBankingMindHub.Service.Instance;
public class AccountService(IAccountRepository _accountRepository, IClientRepository _clientRepository) : IAccountService
{
    public IEnumerable<AccountDTO>? GetAll(out int StatusCode, out string? message)
    {
        var accounts = _accountRepository.GetAllAccounts();

        if (accounts is not null)
        {

            AccountDTO[] accountDTOs = new AccountDTO[accounts.Count()];
            int index = 0;

            foreach (Account account in accounts)
            {
                int TransactionIndex = 1;
                accountDTOs[index] = new()
                {
                    ID = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationTime,
                    Balance = account.Balance,


#pragma warning disable
                    Transactions = account.Transactions.Select(transaction => new TransactionDTO
                    {
                        ID = TransactionIndex++.ToString(),
                        Type = transaction.Type.ToString(),
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Date = transaction.Date,
                        AccountId = transaction.AccountId
                    })
#pragma warning restore
                };
                index++;
            }
            message = null;
            StatusCode = 200;
            return accountDTOs;
        }

        StatusCode = 200;
        message = "No hay usuarios cargados";
        return null;
    }

    public IEnumerable<AccountDTO>? GetAllAcountsByEmail(ClaimsPrincipal claims, out int StatusCode, out string? message)
    {
        string? UserEmail = claims.FindFirst("Client")?.Value;

        if (UserEmail is not null)
        {
            message = null;
            StatusCode = 200;
            Account[]? accounts = _accountRepository.GetAccountsByClient(UserEmail).ToArray();
            AccountDTO[] accountDTOs = new AccountDTO[accounts.Count()];
            int index = 0;
            

            foreach (Account account in accounts)
            {
                int TransactionIndex = 1;
                accountDTOs[index] = new()
                {
                ID = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationTime,
                    Balance = account.Balance,


#pragma warning disable
                    Transactions = account.Transactions.Select(transaction => new TransactionDTO
                    {
                        ID = TransactionIndex++.ToString(),
                        Type = transaction.Type.ToString(),
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        Date = transaction.Date,
                        AccountId = transaction.AccountId
                    })
#pragma warning restore
                };
                index++;
            }


            return accountDTOs;
        }
        else
        {
            message = "El usuario no se ha encontrado.";
            StatusCode = 401;
        }

        return null;
    }

    public AccountDTO? GetByID(string id, out int StatusCode, out string? message)
    {
        Account? account = _accountRepository.FindByID(id);

        if (account is not null)
        {
            message = null;
            StatusCode = 200;
            int transactionIndex = 1;
            return new AccountDTO
            {
                ID = account.Id,
                Number = account.Number,
                CreationDate = account.CreationTime,
                Balance = account.Balance,
#pragma warning disable
                Transactions = account.Transactions.Select(transaction => new TransactionDTO
                {
                    ID = transactionIndex++.ToString(),
                    Type = transaction.Type.ToString(),
                    Amount = transaction.Amount,
                    Description = transaction.Description,
                    Date = transaction.Date,
                    AccountId = transaction.AccountId
                })
#pragma warning restore
            };
        }

        StatusCode = 200;
        message = "No se ha encontrado la cuenta.";
        return null;
    }

    public AccountDTO? PostAccount(ClaimsPrincipal claims, out int StatusCode, out string? message)
    {
        string? Id = claims.FindFirstValue("Client");

        if (Id is not null)
        {
            try
            {
                Client? user = _clientRepository.FindByEmail(Id);


                if (user is not null)
                {
                    bool canPost = _accountRepository.CanPostNewAccount(Id);
                    if (canPost)
                    {
                        Random random = new();
                        string AccountNumber = Utils.Utils.GenerateAccountNumber(random, ref _accountRepository);
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

                        StatusCode = 201;
                        message = "La cuenta se ha creado satisfactoriamente.";
                        return new AccountDTO()
                        {
                            ID = account.Id,
                            Number = account.Number,

                            CreationDate = account.CreationTime,
                            Balance = account.Balance
                        };
                    }

                    else
                    {
                        StatusCode = 403;
                        message = "No se ha podido crear una cuenta bancaria, ya posee el maximo de cuentas.";
                    }
                }
                else
                {
                    StatusCode = 403;
                    message = "No se ha encontrado su usuario.";
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
            StatusCode = 401;
            message = "No se ha podido validar su cuenta";
        }
        return null;
    }
}