using System.Security.Claims;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Auth;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HomeBankingMindHub.Service.Instance;

public class AuthService(IClientRepository clientRepository) : IAuthService
{
    public ClaimsIdentity? Login(LoginModel model, out int statusCode, out string? message)
    {
        try
        {
            Client? user = clientRepository.FindByEmail(model.Email);

            if (user is not null)
            {
                if (Utils.Utils.AreEqual(password: model.Password, passwordHash: user.Password))
                {
                    ClaimsIdentity claimsIdentity = new(claims: [new Claim("Client", user.Email)], authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
                    message = null;
                    statusCode = 200;
                    return claimsIdentity;
                }
                else
                {
                    statusCode = 400;
                }
            }
            else
            {
                statusCode = 401;
            }
            message = "No se ha podido ingresar, por favor revisa lo ingresado.";
        }
        catch (Exception ex)
        {
            message = ex.ToString();
            statusCode = 500;
        }
        return null;
    }
}