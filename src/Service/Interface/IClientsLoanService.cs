using System.Security.Claims;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Loan;

namespace HomeBankingMindHub.Service.Interface;

public interface IClientsLoanService
{
    public ClientsLoanDTO? Post(
        LoanApplicationModel model
        ,ClaimsPrincipal User
        , out int statusCode
        , out string? message);    
}