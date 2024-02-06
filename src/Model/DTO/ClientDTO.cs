using System.Text.Json.Serialization;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Model.DTO;

public class ClientDTO
{
    public int ID {get;set;}
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    #pragma warning disable   
    public ICollection<AccountDTO> Accounts { get; set; }
    public ICollection<ClientsLoanDTO> Loans { get; set; }
    #pragma warning restore 
}