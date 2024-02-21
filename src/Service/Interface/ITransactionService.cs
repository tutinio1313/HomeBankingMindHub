using System.Security.Claims;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Transaction;

namespace HomeBankingMindHub.Service.Interface;

public interface ITransactionService
{
    public IEnumerable<TransactionDTO>? Post(string email,PostTransactionModel model, out int statusCode, out string? message);   
}