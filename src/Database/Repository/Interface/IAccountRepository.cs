using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IAccountRepository {
    IEnumerable<Account> GetAllAccounts();
    IEnumerable<Account> GetAccountsByClient(string Email);
    Account GetAccountByNumber(string Number);
    Account? FindByID(string ID);
    bool ExistsAccountByNumber(string Number);
    bool CanPostNewAccount(string Email);
    void Put(Account account);
    int Save(Account account);
}