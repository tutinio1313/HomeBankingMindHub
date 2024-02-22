using System.Security.Claims;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;
using Microsoft.IdentityModel.Tokens;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Model.Loan;
using NuGet.Configuration;

namespace HomeBankingMindHub.Service.Instance;

public class ClientsLoanService(
                            IClientRepository clientRepository
                            , IClientLoanRepository clientLoanRepository
                            , ITransactionRepository transactionRepository
                            , ILoanRepository loanRepository
                            , IAccountRepository accountRepository)
: IClientsLoanService
{
    public ClientsLoanDTO? Post(
        LoanApplicationModel model
        , string UserEmail
        , out int statusCode
        , out string? message)
    {

        Client? client = clientRepository.FindByEmail(UserEmail);
        if (client is not null)
        {
            Loan? loan = loanRepository.GetByID(model.LoanID);
            if (loan is not null)
            {
                if (loan.MaxAmount >= model.Amount)
                {
                    if (loan.Payment.Contains(model.Payment))
                    {
                        Account? account = accountRepository.GetAccountByNumber(model.NumberAccount);

                        if (account is not null)
                        {
                            if (account.ClientId == client.Id)
                            {
                                Transaction transaction = LoadTransaction(account, model, loan);
                                transactionRepository.Save(transaction);

                                account.SetBalance(transaction.Amount);
                                accountRepository.Put(account);

                                ClientsLoan clientsLoan = LoadClientsLoan(client, model, loan);
                                clientLoanRepository.Save(clientsLoan);

                                statusCode = 201;
                                message = null;

                                return new ClientsLoanDTO()
                                {
                                    ID = clientsLoan.ID
                                    ,
                                    Name = loan.Name
                                    ,
                                    Amount = clientsLoan.Amount
                                    ,
                                    Payments = clientsLoan.Payment
                                };
                            }

                            else
                            {
                                statusCode = 403;
                                message = "La cuenta ingresada no es del cliente.";
                            }
                        }
                        else
                        {
                            statusCode = 403;
                            message = "No se ha encontrado la cuenta del cliente.";
                        }
                    }
                    else
                    {
                        statusCode = 403;
                        message = "Las cuotas ingresadas del prestamo no son validas.";
                    }
                }
                else
                {
                    statusCode = 403;
                    message = "El prestamo solicitado excede el maximo permitido.";
                }
            }

            else
            {
                statusCode = 403;
                message = "El prestamo solicitado no se ha podido encontrar.";
            }
        }
        else
        {
            statusCode = 403;
            message = "No se ha podido encontrar el usuario.";
        }

        return null;
    }


    private Transaction LoadTransaction(Account account
                                        , LoanApplicationModel model
                                        , Loan loan) => new()
                                        {
                                            ID = Guid.NewGuid().ToString()
        ,
                                            Type = TransactionType.CREDIT
        ,
                                            Date = DateTime.Now
        ,
                                            Description = string.Concat(loan.Name, " - Loan approved!")
        ,
                                            Amount = model.Amount
        ,
                                            Account = account
        ,
                                            AccountId = account.Id
                                        };

    private ClientsLoan LoadClientsLoan(Client client
                                        , LoanApplicationModel model
                                        , Loan loan) => new()
                                        {
                                            ID = Guid.NewGuid().ToString()

        ,
                                            Amount = model.Amount * 1.2
        ,
                                            Payment = model.Payment

        ,
                                            Client = client
        ,
                                            ClientID = client.Id

        ,
                                            Loan = loan
        ,
                                            LoanID = loan.ID
                                        };
}