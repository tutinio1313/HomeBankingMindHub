using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;
public class HomeBankingRepository : IHomeBankingRepository
{
    private HomeBankingContext _context;
    public HomeBankingRepository(HomeBankingContext context){
        _context = context;
    }    
    public IQueryable<User> Users => _context.Users;
}