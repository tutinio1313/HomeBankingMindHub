using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface IClientLoanRepository
{
    public bool IsIDAvailable(string id);
    public void Save(ClientsLoan clientLoan);
}