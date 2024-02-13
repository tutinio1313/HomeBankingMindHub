using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public class TransactionRepository(HomeBankingContext context) : Repository<Transaction>(context), ITransactionRepository
{
    public Transaction? FindByID(string id) => FindByCondition(transaction => transaction.ID == id)
                                               .FirstOrDefault();

    public void Save(Transaction transaction)
    {
        if (FindByID(transaction.ID) is not null)
        {
            do
            {
                transaction.ID = Guid.NewGuid().ToString();
            } while (FindByID(transaction.ID) is not null);
        };
        Create(transaction);
        SaveChanges();
    }
}