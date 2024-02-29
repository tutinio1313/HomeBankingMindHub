using Microsoft.AspNetCore.Authentication.Cookies;
using HomeBankingMindHub.Model.Model.Client;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
IConfiguration Configuration = builder.Configuration;

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

//DBInitialazer.PopulateDB(app);
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

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseDefaultFiles();
app.UseStaticFiles();

//Client Service endpoints.
app.MapGet("/api/Client", async () =>
{
    HttpClient client = new();
    using HttpResponseMessage response = await client.GetAsync(Configuration["Services:Client"]);
    if (response.IsSuccessStatusCode)
    {
        return await response.Content.ReadAsStringAsync();
    }
    return null;
});
app.MapGet("/api/Client/{id}", async (string id) =>
{
    HttpClient client = new();
    using HttpResponseMessage response = await client.GetAsync(Configuration["Services:Client"] + $"/{id}");

    if (response.IsSuccessStatusCode)
    {
        return await response.Content.ReadAsStringAsync();
    }
    return null;
});

app.MapPost("/api/Client", async (PostModel model) =>
{
    HttpClient client = new();
    using StringContent content = new(JsonSerializer.Serialize(model));
    using HttpResponseMessage response = await client.PostAsync(Configuration["Services:Client"], content);
       
    if(response.IsSuccessStatusCode)
    {
        return await response.Content.ReadAsStringAsync();
    }
    return null;
});
//Account Service endpoints.

app.MapGet("/api/Account", async () =>
{
    HttpClient client = new();
    using HttpResponseMessage response = await client.GetAsync(Configuration["Services:Client"]);

    if(response.IsSuccessStatusCode)
    {
        return await response.Content.ReadAsStringAsync();
    }
    return null;
});
app.MapGet("/api/Account/{id}", async (string id) =>
{
    HttpClient client = new();
    using HttpResponseMessage response = await client.GetAsync(Configuration["Services:Client" + $"/{id}"]);

    if (response.IsSuccessStatusCode)
    {
        return await response.Content.ReadAsStringAsync();
    }
    return null;
});
app.MapGet("/api/Client/current/account", async (ClaimsPrincipal user) => {
    string? email = user.FindFirstValue("Client");

    if (!email.IsNullOrEmpty())
    {
        HttpClient client = new();
        using HttpResponseMessage response = await client.GetAsync( requestUri: Configuration["Services:Client"]+ "current/account");
        return response.Content.ReadAsStringAsync();
    }

    return null;
}).RequireAuthorization();

app.MapPost("/api/Client/current/accounts", async (ClaimsPrincipal user) => {
    string? email = user.FindFirstValue("Client");

    if (!email.IsNullOrEmpty()) 
    {
        HttpClient client = new();
        using StringContent content = new(JsonSerializer.Serialize(email));
        using HttpResponseMessage response = await client.PostAsync(Configuration["Services:Client"] + "current/accounts", content);
        return response.Content.ReadAsStringAsync();
    }
    return null;
}).RequireAuthorization();
//Card Service endpoints.

app.MapGet("/api/Client/current/cards", async (ClaimsPrincipal user) =>
{
    string? email = user.FindFirstValue("Client");

    if (!email.IsNullOrEmpty())
    {
        HttpClient client = new();
        using StringContent content = new(JsonSerializer.Serialize(email));
        using HttpResponseMessage response = await client.PostAsync(Configuration["Services:Client"]+ "current/cards", content);

        if(response.IsSuccessStatusCode)
        {
        return response.Content.ReadAsStringAsync();
        }
    }
    return null;
});
app.MapPost("/api/Client/current/cards", async (ClaimsPrincipal user) =>
{
    string? email = user.FindFirstValue("Client");

    if (!email.IsNullOrEmpty())
    {
        HttpClient client = new();
        using StringContent content = new(JsonSerializer.Serialize(email));
        using HttpResponseMessage response = await.PostAsync(Configuration["Services:Client"] + "current/cards", content);
    }
    return null;
});
//Auth Service endpoints.

app.Run();
