
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HomeBankingMindHub.Model.Entity;

public class User
{
    [Key]
    public required Guid guid { get; set; }
    public required string FirstName {get;set;}
    public required string LastName { get;set;}
    public required string Email { get;set;}
    public required string Password { get;set;}
}
