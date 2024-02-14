using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Transaction;
using HomeBankingMindHub.Model.DTO;

using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace HomeBankingMindHub.Service.Instance;

public class TransactionService(ITransactionRepository transactionRepository
                                ,IAccountRepository accountRepository
                                ,IClientRepository clientRepository) : ITransactionService
{
    public IEnumerable<TransactionDTO>? Post(ClaimsPrincipal claims, PostTransactionModel model, out int statusCode, out string? message)
    {
        try
        {
            if (!model.FromAccountNumber.Equals(model.ToAccountNumber))
            {
                string? UserEmail = claims.FindFirst("Client")?.Value;
                if (UserEmail is not null)
                {
                    Account? fromAccount = accountRepository.GetAccountByNumber(Number: model.FromAccountNumber.ToUpper());
                    Account? toAccount = accountRepository.GetAccountByNumber(Number: model.ToAccountNumber.ToUpper());

                    if (fromAccount is not null && toAccount is not null)
                    {
                        if (clientRepository.FindByID(fromAccount.ClientGuid).Email.Equals(UserEmail))
                        {
                            if (fromAccount.Balance - model.Amount >= 0)
                            {
                                fromAccount.SetBalance(-model.Amount);
                                toAccount.SetBalance(model.Amount);

                                accountRepository.Put(fromAccount);
                                accountRepository.Put(toAccount);

                                Transaction fromTransaction = LoadTransaction( model: model, account:fromAccount, type: TransactionType.DEBIT);
                                Transaction toTransaction =  LoadTransaction( model: model, account:toAccount, type: TransactionType.CREDIT);

                                transactionRepository.Save(fromTransaction);
                                transactionRepository.Save(toTransaction);

                                message = null;
                                statusCode = 201;
                                return [LoadTransactionDTO(1, fromTransaction), LoadTransactionDTO(2,toTransaction)];
                            }
                            else
                            {
                                message = "No tiene fondos suficientes.";
                                statusCode = 403;
                            }
                        }
                        else
                        {
                            message = "La cuenta ingresada no le pertenece al usuario ingresado.";
                            statusCode = 401;
                        }
                    }
                    else
                    {
                        if (fromAccount is null && toAccount is null) message = "Las cuentas ingresadas no son validas.";
                        else if (toAccount is null) message = "La cuenta receptora no es valida.";
                        else message = "La cuenta emisora no es valida.";
                        statusCode = 400;
                    }
                }
                else
                {
                    message = "No se ha encontrado el usuario.";
                    statusCode = 401;
                }
            }
            else
            {
                message = "Parece que has ingresado dos cuentas iguales, selecciona otra cuenta de receptora que no sea la misma que la emisora.";
                statusCode = 400;
            }
        }
        catch (Exception ex)
        {
            message = ex.ToString();
            statusCode = 500;
        }

        return null;
    }

    private static Transaction LoadTransaction(PostTransactionModel model, Account account, TransactionType type) => new()
    {
        ID = Guid.NewGuid().ToString()
        ,Type = type
        ,Description = model.Description
        ,Date = DateTime.Now
        ,Account = account
        ,AccountId = account.Id
        ,Amount = model.Amount
    };
    private static TransactionDTO LoadTransactionDTO(int index, Transaction transaction) => new()
    {
        ID = index.ToString()
            ,
        Type = nameof(transaction.Type)
            ,
        Amount = transaction.Amount
            ,
        Description = transaction.Description
            ,
        Date = transaction.Date
            ,
        AccountId = transaction.AccountId
    };
}