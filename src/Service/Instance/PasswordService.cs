using System.Security.Cryptography;
using System.Text;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Service.Instance;

public class PasswordService:IPasswordService {
    public string HashPassword(string password) => Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(password)));

    public bool AreEqual(string password, string passwordHash) => HashPassword(password).Equals(passwordHash);
}