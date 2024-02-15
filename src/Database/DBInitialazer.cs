using System.Security.Cryptography;
using System.Text;
using HomeBankingMindHub.Database;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Service.Instance;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Versioning;
using static System.Net.Mime.MediaTypeNames;

namespace HomeBankingMindHub.Database;

public static class DBInitialazer
{
    private static Client[] users = [
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Andrés", LastName = "Rossini", Email = "andres@aol.com", Password = "tuti1313" },
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Andrea", LastName = "Rossina", Email = "andrea@yahoo.com", Password = "yahoo123" },
        new() { Id = Guid.NewGuid().ToString(), FirstName = "Victoria", LastName = "Sanchez", Email = "SanVick@aola.com", Password = "asdasdasd" },
        new () { Id = "1", FirstName = "Victor", LastName = "Coronado", Email = "vcoronado@gmail.com", Password = "123456" }
    ];

    private static readonly Account[] accounts = [
        new () {Id = "1", CreationTime = DateTime.Now, Number = "VIN-00000001", Client = users[3], ClientId = users[3].Id},
        new () {Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "VIN-00000002", Client = users[3], ClientId = users[3].Id, Balance = 13500},
        new(){ Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "VIN-00000003", Client = users[0], ClientId = users[0].Id}
    ];

    private static readonly Transaction[] transactions = [
        new() { ID = "1", Type = TransactionType.CREDIT, Date = DateTime.Now.AddHours(-2), Description = "Transferencia recibida", AccountId = "1", Account = accounts[0], Amount = 10000}
        , new () {ID = "2", Type = TransactionType.DEBIT, Date = DateTime.Now.AddHours(-3), Description = "Compra en tienda de Mercado Libre", AccountId = "1", Account = accounts[0], Amount = -2000}
        , new() {ID = "3", Type = TransactionType.DEBIT, Date = DateTime.Now.AddHours(-4), Description = "Compra en tienda", AccountId = "1", Account = accounts[0], Amount = -3000}
    ];

    private static readonly Loan[] loans = {
        new() { ID = "1", Name = "Hipotecario", MaxAmount = 5000, Payment = "12,24,36,48,60"},
        new(){ ID = "2", Name = "Personal", MaxAmount = 100000, Payment = "6,12,24" },
        new() {ID = "3",  Name = "Automotriz", MaxAmount = 300000, Payment = "6,12,24,36" }
    };

    private static ClientsLoan[] clientsLoans = {
        new() {ID = "1", Amount = 400000, LoanID = "1", ClientID = "1", Client = users[3] , Loan = loans[0], Payment = "60" },
        new() {ID = "2", Amount = 50000, LoanID = "2", ClientID = "1", Client = users[3], Loan = loans[1],  Payment = "12" },
        new () {ID = "3", Amount = 100000, LoanID = "3", ClientID = "1", Client = users[3], Loan = loans[2], Payment = "24"}
    };

    private static Card[] cards = {
        new() {
            Id = "1" ,
            ClientID = users[3].Id,
            client = users[3],
            CardHolder = users[3].FirstName + " " + users[3].LastName,
            Type = CardType.DEBIT,
            Color = CardColor.GOLD,
            Number = "3325-6745-7876-4445",
            CVV = 990,
            FromDate= DateTime.Now,
            ThruDate= DateTime.Now.AddYears(4),
        },
        new() {
            Id = "2",
            ClientID = users[3].Id,
            client = users[3],
            CardHolder = users[3].FirstName + " " + users[3].LastName,
            Type = CardType.CREDIT,
            Color = CardColor.TITANIUM,
            Number = "2234-6745-552-7888",
            CVV = 750,
            FromDate= DateTime.Now,
            ThruDate= DateTime.Now.AddYears(5),
        },
    };

#pragma warning disable
    private static HomeBankingContext context;
#pragma warning restore
    public static void CreateContext(IApplicationBuilder app)
    {
        context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HomeBankingContext>();
    }

    public static void PopulateDB(IApplicationBuilder app)
    {
        CreateContext(app);

        LoadUsers();
        LoadAccounts();

        LoadLoans();
        LoadClientLoans();

        LoadCards();
    }

    private static void LoadUsers()
    {
        if (!context.Clients.Any())
        {
            HashPassword();
            context.Clients.AddRange(users);

            context.SaveChanges();
        }
    }

   

    private static void HashPassword(){
        foreach(Client user in users) {
            user.Password = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(user.Password)));
        }
    }

    private static void LoadAccounts()
    {

        if (!context.Accounts.Any())
        {
            context.Accounts.AddRange(accounts);
            context.SaveChanges();
            LoadTransactions();
        }
    }

    private static void LoadTransactions()
    {
        if (!context.Transactions.Any())
        {
            foreach(Transaction transaction in transactions)
            {
                accounts.First(x => x.Id == transaction.AccountId).SetBalance(transaction.Amount);
            }
            context.Transactions.AddRange(transactions);
            context.Accounts.UpdateRange(accounts);
            context.SaveChanges();
        }
    }

    public static void SetAccountBalance()
    {
        foreach (Transaction transaction in context.Transactions.ToList())
        {
            transaction.Account.SetBalance(transaction.Amount);
        }
        context.SaveChanges();
    }

    private static void LoadLoans()
    {
        if (!context.Loans.Any())
        {
            context.Loans.AddRange(loans);
            context.SaveChanges();
        }
    }
    private static void LoadClientLoans()
    {
        if (!context.ClientLoans.Any())
        {
            context.ClientLoans.AddRange(clientsLoans);
            context.SaveChanges();
        }

    }

    private static void LoadCards()
    {
        if (!context.Cards.Any())
        {
            context.Cards.AddRange(cards);
            context.SaveChanges();
        }
    }
}
