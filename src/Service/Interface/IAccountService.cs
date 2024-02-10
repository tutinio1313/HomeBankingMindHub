using System.Security.Claims;
using HomeBankingMindHub.Model.DTO;

namespace HomeBankingMindHub.Service.Interface;

public interface IAccountService {
    public IEnumerable<AccountDTO>? GetAll(out int StatusCode, out string? message);

    public AccountDTO? GetByID(string id, out int StatusCode, out string? message);    
    public AccountDTO? PostAccount(ClaimsPrincipal claims, out int StatusCode, out string? message);
}