using HomeBankingMindHub.Model.DTO;

namespace HomeBankingMindHub.Service.Interface;

public interface ILoanService
{
    public IEnumerable<LoanDTO>? GetAll(out int statusCode, out string? message);    
}