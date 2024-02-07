using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Model.DTO;

public class CardDTO
{
    public required string Id {get;set;}
    public required string CardHolder {get;set;}
    public required string Type {get;set;}
    public required string Color {get;set;}
    public required string Number {get;set;}
    public required int CVV {get;set;}
    public required DateTime FromDate {get;set;}
    public required DateTime ThruDate {get;set;}    

}