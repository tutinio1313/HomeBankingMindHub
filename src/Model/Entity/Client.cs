
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeBankingMindHub.Model.Entity;

public class Client
{
    [Key]
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    
    // [NotMapped]
    public ICollection<Account> Accounts { get; set; }
}
