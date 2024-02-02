using Microsoft.EntityFrameworkCore;
using HomeBankingMindHub.Model.Entity;

namespace HomeBankingMindHub.Database;

public class HomeBankingContext : DbContext
{
    public HomeBankingContext(DbContextOptions<HomeBankingContext> options) : base(options) { }
    public DbSet<User> Users {get;set;}
}