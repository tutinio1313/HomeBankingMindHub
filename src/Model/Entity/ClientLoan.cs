using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Entity;

public class ClientsLoan {
    [Key]
    public required string ID { get; set; }
    public required double Amount { get; set; }
    public required string Payment {get;set;}
    public required string ClientID {get;set;}
    public Client Client {get;set;}

    public Loan Loan { get; set; }    
    public required string LoanID { get; set; }    
}