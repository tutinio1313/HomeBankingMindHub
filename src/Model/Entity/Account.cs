using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeBankingMindHub.Model.Entity;

public class Account
{
    [Key]
    public required string Id {get;set;}
    public required DateTime CreationTime {get;set;}
    public required string Number {get;set;}
    public double Balance {get; set;} = 0.00;
    [ForeignKey("Client")]
    public required string ClientId {get;set;}
    public required Client Client {get; set;}
    public IEnumerable<Transaction>? Transactions {get;set;}

    public void SetBalance(double amount) => Balance +=amount;
}
