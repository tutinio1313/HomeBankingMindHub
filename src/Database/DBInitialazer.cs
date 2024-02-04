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
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Victoria", LastName = "Sanchez", Email = "SanVick@aola.com", Password = "asdasdasd" }
    ];

    private static readonly Account[] accounts = [
        new(){ Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "ACC-0", Client = users[0], ClientGuid = users[0].Id}
    ];

    private static HomeBankingContext context;

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
