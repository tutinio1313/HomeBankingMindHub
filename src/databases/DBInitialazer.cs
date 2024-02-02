using HomeBankingMindHub.Database;
using HomeBankingMindHub.Model.Entity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using static System.Net.Mime.MediaTypeNames;

namespace HomeBankingMindHub.Database;

public static class DBInitialazer
{
    private static readonly User[] users = [ 
        new() { guid = new Guid(), FirstName = "Andrés", LastName = "Rossini", Email = "andres@aol.com", Password = "tuti1313" },
        new() { guid = new Guid(), FirstName = "Andrea", LastName = "Rossina", Email = "andrea@yahoo.com", Password = "yahoo123" },
        new() { guid = new Guid(), FirstName = "Victoria", LastName = "Sanchez", Email = "SanVick@aola.com", Password = "asdasdasd" }
    ];

    public static void LoadUsers(IApplicationBuilder app)
    {
        HomeBankingContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<HomeBankingContext>();

        if (!context.Users.Any())
        {
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }

}
