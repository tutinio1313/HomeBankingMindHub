using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;

using HomeBankingMindHub.Database;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Service.Instance;
using HomeBankingMindHub.Service.Interface;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<HomeBankingContext>(options =>
{
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("HomeBankingConnection"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPasswordService, PasswordService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                            {
                                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                                options.LoginPath = new PathString("/index.html");
                            });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

DBInitialazer.PopulateDB(app);

//DBInitialazer.SetAccountBalance();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
