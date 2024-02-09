using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IAccountRepository {
    IEnumerable<Account> GetAllAccounts();
    IEnumerable<Account> GetAccountsByClient(string Id);
    Account? FindByID(string ID);
    bool ExistsAccountByNumber(string Number);
    int Save(Account account);
}