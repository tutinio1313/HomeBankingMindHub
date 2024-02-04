using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;
public class HomeBankingRepository : IHomeBankingRepository
{
    private HomeBankingContext _context;
    public HomeBankingRepository(HomeBankingContext context){
        _context = context;
    }    
    public IQueryable<Client> Clients => _context.Clients;
    public IQueryable<Account> Accounts => _context.Accounts;
    
}