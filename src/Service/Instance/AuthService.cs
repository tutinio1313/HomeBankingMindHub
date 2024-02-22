using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Auth;
using HomeBankingMindHub.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HomeBankingMindHub.Service.Instance;

public class AuthService(IClientRepository clientRepository, IKeyService keyService) : IAuthService
{
    // public ClaimsIdentity? Login(LoginModel model, out int statusCode, out string? message)
    // {
    //     try
    //     {
    //         Client? user = clientRepository.FindByEmail(model.Email);

    //         if (user is not null)
    //         {
    //             if (Utils.Utils.AreEqual(password: model.Password, passwordHash: user.Password))
    //             {
    //                 ClaimsIdentity claimsIdentity = new(claims: [new Claim("Client", user.Email)], authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
    //                 message = null;
    //                 statusCode = 200;
    //                 return claimsIdentity;
    //             }
    //             else
    //             {
    //                 statusCode = 400;
    //             }
    //         }
    //         else
    //         {
    //             statusCode = 401;
    //         }
    //         message = "No se ha podido ingresar, por favor revisa lo ingresado.";
    //     }
    //     catch (Exception ex)
    //     {
    //         message = ex.ToString();
    //         statusCode = 500;
    //     }
    //     return null;
    // }
    private readonly SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0C4419ABB0723453DBDC5EB8FC935621C5B2B81EA5D12EF1EBF24C1D39FB18CC"));
    public string? Login(LoginModel model, out int statusCode, out string? message)
    {
        try
        {
            Client? user = clientRepository.FindByEmail(model.Email);

            if (user is not null)
            {
                if (Utils.Utils.AreEqual(password: model.Password, passwordHash: user.Password))
                {
                    message = null;
                    statusCode = 200;
                    return keyService.GenerateToken(user);
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