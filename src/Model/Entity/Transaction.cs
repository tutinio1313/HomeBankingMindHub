namespace HomeBankingMindHub.Model.Entity;

public class Transaction
{
    public required string ID {get;set;}
    public required TransactionType Type {get;set;}
    public required double Amount {get;set;}
    public required string Description {get; set;}
    public required DateTime Date {get; set;}
    public required Account Account {get;set;}
    public required string AccountId {get;set;}
}

public enum TransactionType { 
    CREDIT,
    DEBIT
}