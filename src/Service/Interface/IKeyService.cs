using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Service.Interface;

public interface IKeyService
{
    public string GenerateToken(Client user);    
}