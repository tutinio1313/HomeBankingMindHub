using Microsoft.EntityFrameworkCore;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;

public class HomeBankingContext : DbContext
{    
    public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }
    
    public DbSet<Client> Clients {get;set;}
    public DbSet<Account> Accounts {get;set;}
    public DbSet<Transaction> Transactions {get;set;}
    public DbSet<Loan> Loans {get;set;}
    public DbSet<Card> Cards {get;set;}
    public DbSet<ClientsLoan> ClientLoans {get;set;}
}