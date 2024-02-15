using HomeBankingMindHub.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Database.Repository;

public class AccountRepository(HomeBankingContext context) : Repository<Account>(context), IAccountRepository
{
    public Account? FindByID(string ID) => FindByCondition(account => account.Id == ID).Include( account => account.Transactions).FirstOrDefault();
    
    public IEnumerable<Account> GetAllAccounts() => FindAll().Include(account => account.Transactions).ToArray();

    public int Save(Account account)
    {
        Create(account);
        return SaveChanges();
    }
}