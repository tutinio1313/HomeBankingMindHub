namespace HomeBankingMindHub.Model.DTO;

public class TransactionDTO  { 
    public required string ID {get;set;}
    public required string Type {get;set;}
    public required double Amount {get;set;}
    public required string Description {get; set;}
    public required DateTime Date {get; set;}
    public required string AccountId {get;set;}
}

