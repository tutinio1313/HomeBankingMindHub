using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeBankingMindHub.Model.Entity;

public class Card
{
    [Key]
    public required string Id {get;set;}
    [ForeignKey("Client")]
    public required string ClientID {get;set;}
    [NotMapped]
    public required Client client {get;set;}
    public required string CardHolder {get;set;}
    public required string Type {get;set;}
    public required string Color {get;set;}
    public required string Number {get;set;}
    public required int CVV {get;set;}
    public required DateTime FromDate {get;set;}
    public required DateTime ThruDate {get;set;}    
}

public enum CardType
{
    DEBIT,
    CREDIT   
}

public enum CardColor {
    GOLD,
    SILVER,
    TITANIUM
}

