namespace HomeBankingMindHub.Service.Interface;

public interface IPasswordService {
    public string HashPassword(string password);
    public bool AreEqual(string password, string passwordHash);
}