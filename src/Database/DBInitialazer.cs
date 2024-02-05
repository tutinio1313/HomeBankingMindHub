using System.Security.Cryptography;
using HomeBankingMindHub.Database;
using HomeBankingMindHub.Model.Entity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace HomeBankingMindHub.Database;

public static class DBInitialazer
{
    private static readonly Client[] users = [
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Andrés", LastName = "Rossini", Email = "andres@aol.com", Password = "tuti1313" },
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Andrea", LastName = "Rossina", Email = "andrea@yahoo.com", Password = "yahoo123" },
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Victoria", LastName = "Sanchez", Email = "SanVick@aola.com", Password = "asdasdasd" },
        new () { Id = "1", FirstName = "Victor", LastName = "Coronado", Email = "VictorMasCapoCoronoado@gmail.com", Password = "vicCor123" }
    ];

    private static readonly Account[] accounts = [
        new(){ Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "ACC-0", Client = users[0], ClientGuid = users[0].Id},
        new () {Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "VIN001", Client = users[3], ClientGuid = users[3].Id, Balance = 5000.30},
        new () {Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "VIN002", Client = users[3], ClientGuid = users[3].Id, Balance = -3500}
    ];

    #pragma warning disable
    private static HomeBankingContext context;
    #pragma warning restore
    public static void CreateContext(IApplicationBuilder app)
    {
        context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HomeBankingContext>();
    }

    public static void LoadUsers()
    {
        if (!context.Clients.Any())
        {
            context.Clients.AddRange(users);
            context.SaveChanges();
        }
    }

    public static void LoadAccounts()
    {

        if (!context.Accounts.Any())
        {

            context.Accounts.AddRange(accounts);
            context.SaveChanges();
        }
    }
}
