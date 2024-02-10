using HomeBankingMindHub.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Database.Repository;

public class AccountRepository(HomeBankingContext context) : Repository<Account>(context), IAccountRepository
{
    public Account? FindByID(string ID) => FindByCondition(account => account.Id == ID)
                                          .Include(account => account.Transactions).FirstOrDefault();
#pragma warning disable
    public IEnumerable<Account>? GetAllAccounts() => [.. FindAll().Include(account => account.Transactions)];
    public IEnumerable<Account>? GetAccountsByClient(string Email) => [.. FindByCondition(x => x.Client.Email == Email).Include(account => account.Transactions)];
#pragma warning restore
    public bool ExistsAccountByNumber(string Number) => FindByCondition(x => x.Number == Number).Any();
    public bool CanPostNewAccount(string Email) => FindByCondition(account => account.Client.Email == Email).Count() < 3;
    
    public int Save(Account account)
    {
        Create(account);
        return SaveChanges();
    }

}