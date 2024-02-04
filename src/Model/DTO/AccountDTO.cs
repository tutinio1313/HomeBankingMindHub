    using System.Text.Json.Serialization;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Model.DTO;

public class AccountDTO
{
    public int ID {get;set;}
    public string Number {get; set;}
    public DateTime CreationTime {get;set;}
    public double Balance {get; set;} = 0.00;

}