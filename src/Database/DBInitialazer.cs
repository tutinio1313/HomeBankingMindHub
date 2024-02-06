using System.Security.Cryptography;
using HomeBankingMindHub.Database;
using HomeBankingMindHub.Model.Entity;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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
        new () {Id = "1", CreationTime = DateTime.Now, Number = "VIN001", Client = users[3], ClientGuid = users[3].Id},
        new () {Id = Guid.NewGuid().ToString(), CreationTime = DateTime.Now, Number = "VIN002", Client = users[3], ClientGuid = users[3].Id, Balance = -3500}
    ];

    private static readonly Transaction[] transactions = [
        new() { ID = "1", Type = TransactionType.CREDIT.ToString(), Date = DateTime.Now.AddHours(-2), Description = "Transferencia recibida", AccountId = "1", Account = accounts[1], Amount = 10000}
        , new () {ID = "2", Type = TransactionType.DEBIT.ToString(), Date = DateTime.Now.AddHours(-3), Description = "Compra en tienda de Mercado Libre", AccountId = "1", Account = accounts[1], Amount = -2000}
        , new() {ID = "3", Type = TransactionType.DEBIT.ToString(), Date = DateTime.Now.AddHours(-4), Description = "Compra en tienda", AccountId = "1", Account = accounts[1], Amount = -3000}
    ];

    private static readonly Loan[] loans = {
        new() { ID = "1", Name = "Hipotecario", MaxAmount = 5000, Payment = "12,24,36,48,60"},
        new(){ ID = "2", Name = "Personal", MaxAmount = 100000, Payment = "6,12,24" },
        new() {ID = "3",  Name = "Automotriz", MaxAmount = 300000, Payment = "6,12,24,36" }
    };

    private static ClientsLoan[] clientsLoans = {
        new() {ID = "1", Amount = 400000, LoanID = "1", ClientID = "1", Payment = "60" },
        new() {ID = "2", Amount = 50000, LoanID = "2", ClientID = "1", Payment = "12" },
        new () {ID = "3", Amount = 100000, LoanID = "3", ClientID = "1", Payment = "24"}
    };

#pragma warning disable
    private static HomeBankingContext context;
#pragma warning restore
    public static void CreateContext(IApplicationBuilder app)
    {
        context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HomeBankingContext>();
    }

    public static void PopulateDB(IApplicationBuilder app){
            CreateContext(app);

            LoadUsers();
            LoadAccounts();

            LoadLoans();
            LoadClientLoans();

    }

    private static void LoadUsers()
    {
        if (!context.Clients.Any())
        {
            context.Clients.AddRange(users);

            context.SaveChanges();
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
            context.Transactions.AddRange(transactions);
            context.SaveChanges();
        }
    }

    public static void SetAccountBalance() {
            foreach(Transaction transaction in context.Transactions.ToList()) {
                transaction.Account.SetBalance(transaction.Amount);
            }
            context.SaveChanges();                
    }

    private static void LoadLoans()
    {
        if(!context.Loans.Any())
        {
            context.Loans.AddRange(loans);
            context.SaveChanges();
        }
    }
    private static void LoadClientLoans(){
        if(!context.ClientLoans.Any())
        {
            context.ClientLoans.AddRange(clientsLoans);
            context.SaveChanges();
        }

    }
}
