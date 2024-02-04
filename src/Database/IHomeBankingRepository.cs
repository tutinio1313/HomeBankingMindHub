
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;

public interface IHomeBankingRepository
{
    IQueryable<Client> Clients {get;}
    IQueryable<Account> Accounts {get;}
}
