using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public interface ILoanRepository
{
    public IEnumerable<Loan> GetAll();
    public Loan? GetByID(string id);
    public void Save(Loan loan);
}