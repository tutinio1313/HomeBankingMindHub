
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;

public interface IHomeBankingRepository
{
    IQueryable<User> Users {get;}
}
