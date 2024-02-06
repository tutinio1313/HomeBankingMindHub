using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IAccountRepository {
    IEnumerable<Account> GetAllAccounts();
    Account? FindByID(string ID);
    int Save(Account account);
}