using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using HomeBankingMindHub.Database;
using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Service.Instance;
using HomeBankingMindHub.Service.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<HomeBankingContext>(options =>
{
    options.UseSqlServer(
    builder.Configuration.GetConnectionString("HomeBankingConnection"));
    options.EnableDetailedErrors();
    options.EnableSensitiveDataLogging();
});

//Repository Services.
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IClientLoanRepository, ClientLoanRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

//Entity Services.
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ILoanService, LoanService>();
builder.Services.AddScoped<IClientsLoanService, ClientsLoanService>();
builder.Services.AddScoped<ICardService, CardService>();

//Auxiliar services.
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IKeyService,KeyService>();

builder.Services.AddControllers();

//Added JWT to project.
builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                            {
                                SymmetricSecurityKey SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                        builder.Configuration["JWTKey"]));
                                SigningCredentials SigningCredentials = new SigningCredentials(
                                        SymmetricSecurityKey
                                        , SecurityAlgorithms.HmacSha256Signature);
                                options.RequireHttpsMetadata = false;
                                options.TokenValidationParameters = new()
                                {
                                    ValidateAudience = false,
                                    ValidateIssuer = false,
                                    ValidateLifetime = true,

                                    IssuerSigningKey = SymmetricSecurityKey
                                };
                            })
                    .AddCookie();
builder.Services.AddAuthorization();

//Some dependencies needed to use Swagger.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


//DBInitialzer to populate DB.
DBInitialazer.PopulateDB(app);
//DBInitialazer.SetAccountBalance();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();
