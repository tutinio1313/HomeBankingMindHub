    using System.Text.Json.Serialization;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Model.DTO;

public class AccountDTO
{
    public string ID {get;set;} = string.Empty;
    public string Number {get; set;} = string.Empty;
    public DateTime CreationTime {get;set;}
    public double Balance {get; set;} = 0.00;
    public IEnumerable<TransactionDTO> Transactions {get;set;}

}