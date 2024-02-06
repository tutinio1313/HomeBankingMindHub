namespace HomeBankingMindHub.Model.DTO;

public class ClientsLoanDTO {
    public required string ID { get; set; }
    public required string LoanID { get; set; }
    public required string Name {get;set;}
    public required double Amount { get; set; }
    public required string Payments {get;set;}
}