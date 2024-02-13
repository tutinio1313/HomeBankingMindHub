using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface ITransactionRepository
{
    void Save(Transaction transaction);
    Transaction? FindByID(string id);
}