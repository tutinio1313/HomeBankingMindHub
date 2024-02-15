using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Service.Instance;

public class LoanService(ILoanRepository repository) : ILoanService
{
    public IEnumerable<LoanDTO>? GetAll(out int statusCode, out string? message)
    {
        try
        {
            IEnumerable<LoanDTO>? LoanDTOs = LoadLoanDTOs();
            statusCode = 200;
            message = LoanDTOs is null ? "No hay prestamos cargados." : null;
            return LoanDTOs;            
        }
        catch(Exception ex)
        {
            statusCode = 500;
            message = ex.ToString();
            return null;
        }
    }

    private IEnumerable<LoanDTO>? LoadLoanDTOs()
    {
        IEnumerable<Loan> loans = repository.GetAll(); 
        
        if(loans is not null && loans.Count() > 0)
        {
            LoanDTO[] loanDTOs = new LoanDTO[loans.Count()]; 
            int index = 0;

            foreach(Loan loan in loans)
            {
                loanDTOs[index] = new LoanDTO() {
                    ID = loan.ID
                    ,Name = loan.Name
                    ,MaxAmount = loan.MaxAmount
                    ,Payments = loan.Payment
                };
                index++;
            }
            return loanDTOs;
        }
        
        return null;
    }
}