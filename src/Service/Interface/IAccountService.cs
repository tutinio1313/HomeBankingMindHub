using System.Security.Claims;
using HomeBankingMindHub.Model.DTO;

namespace HomeBankingMindHub.Service.Interface;

public interface IAccountService {
    public IEnumerable<AccountDTO>? GetAll(out int StatusCode, out string? message);
    public IEnumerable<AccountDTO>? GetAllAcountsByEmail(string UserEmail ,out int StatusCode, out string? message);

    public AccountDTO? GetByID(string id, out int StatusCode, out string? message);    
    public AccountDTO? PostAccount(string UserEmail, out int StatusCode, out string? message);
}