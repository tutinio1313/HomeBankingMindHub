using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using HomeBankingMindHub.Database;
using HomeBankingMindHub.Database.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<HomeBankingContext>(options =>
{
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("HomeBankingConnection"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();

var app = builder.Build();

DBInitialazer.CreateContext(app);
DBInitialazer.LoadUsers();
DBInitialazer.LoadAccounts();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
