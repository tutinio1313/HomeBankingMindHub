using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database.Repository;

public class LoanRepository(HomeBankingContext context) : Repository<Loan>(context), ILoanRepository
{
    public IEnumerable<Loan> GetAll() => FindAll();

    public Loan? GetByID(string id) => FindByCondition(loan => loan.ID == id).FirstOrDefault();
    public bool IsIDAvailable(string id) => !FindByCondition(loan => loan.ID == id).Any();

    public void Save(Loan loan)
    {
        if(!IsIDAvailable(loan.ID))
        {
            do
            {
                loan.ID = Guid.NewGuid().ToString();
            } while (!IsIDAvailable(loan.ID));
        }
        Create(loan);
        SaveChanges();
    }
}