using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Entity;

public class Client
{
    [Key]
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    public ICollection<Account>? Accounts { get; set; }
    public ICollection<ClientsLoan>? Loans { get; set; }
    public ICollection<Card>? Cards { get; set; }
}
