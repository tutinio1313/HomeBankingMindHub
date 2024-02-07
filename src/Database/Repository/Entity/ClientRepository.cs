using HomeBankingMindHub.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Database.Repository;

public class ClientRepository(HomeBankingContext context) : Repository<Client>(context), IClientRepository
{
    public Client? FindByID(string ID) => FindByCondition(client => client.Id == ID
    ).Include(client => client.Accounts)
    .Include(client => client.Cards)
    .Include(client => client.Loans)
    .ThenInclude(client => client.Loan)
    .FirstOrDefault();
    public Client? FindByEmail(string Email) => FindByCondition(client => client.Email == Email)
    .Include(client => client.Accounts)
    .Include(client => client.Cards)
    .Include(client => client.Loans)
    .ThenInclude(client => client.Loan)
    .FirstOrDefault();
    
    public IEnumerable<Client> GetAllUsers() => FindAll()
    .Include(client => client.Accounts)
    .Include(client => client.Cards)
    .Include(client => client.Loans)
    .ThenInclude(client => client.Loan)
        .ToArray();

    public int Save(Client client)
    {
        Create(client);
        return SaveChanges();
    }
}