using HomeBankingMindHub.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Database.Repository;

public class AccountRepository(HomeBankingContext context) : Repository<Account>(context), IAccountRepository
{
    public Account? FindByID(string ID) => FindByCondition(account => account.Id == ID).FirstOrDefault();
    
    public IEnumerable<Account> GetAllAccounts() => FindAll().ToArray();

    public int Save(Account account)
    {
        Create(account);
        return SaveChanges();
    }
}