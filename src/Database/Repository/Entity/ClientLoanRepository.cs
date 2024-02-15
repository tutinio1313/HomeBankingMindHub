using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public class ClientLoanRepository(HomeBankingContext context) : Repository<ClientsLoan>(context), IClientLoanRepository
{
    public bool IsIDAvailable(string id) => !FindByCondition(clientLoan => clientLoan.ID == id).Any();

    public void Save(ClientsLoan clientLoan)
    {
        if (!IsIDAvailable(clientLoan.ID))
        {
            do
            {
                clientLoan.ID = Guid.NewGuid().ToString();
            } while (!IsIDAvailable(clientLoan.ID));
        }

        Create(clientLoan);
        SaveChanges();
    }
}