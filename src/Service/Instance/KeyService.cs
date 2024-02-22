using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Model.Entity;

using Microsoft.IdentityModel.Tokens;
using System.Configuration;

namespace HomeBankingMindHub.Service.Instance;

public class KeyService(IConfiguration config) : IKeyService
{
#pragma warning disable
    private readonly SymmetricSecurityKey Key = new(Encoding.UTF8.GetBytes(config["JWTKey"]));
#pragma warning restore
    public string GenerateToken(Client user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                            new(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(15),
            SigningCredentials = new(Key, SecurityAlgorithms.HmacSha256Signature)
        };
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}