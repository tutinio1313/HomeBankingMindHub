using System.Security.Claims;
using HomeBankingMindHub.Model.Model.Auth;

namespace HomeBankingMindHub.Service.Interface;
public interface IAuthService
{
    public ClaimsIdentity? Login(LoginModel model, out int statusCode, out string? message);
}