using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Entity;

public class Loan
{
    [Key]
    public required string ID { get; set; }
    public required string Name { get; set; }
    public required double MaxAmount { get; set; }
    public required string Payment { get; set; }
    public IEnumerable<ClientsLoan>? ClientsLoans;
    
}