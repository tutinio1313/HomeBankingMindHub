using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Client;
using HomeBankingMindHub.Model.DTO;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountRepository accountRepository) : ControllerBase
    {
        private readonly IAccountRepository accountRepository = accountRepository;

        [HttpGet]
        public ActionResult<IEnumerable<AccountDTO>> Get()
        {
            var accounts = accountRepository.GetAllAccounts();

            if(accounts is not null) {
                
                AccountDTO[] accountDTOs = new AccountDTO[accounts.Count()];
                int index = 0;

                foreach(Account account in accounts) {
                    accountDTOs[0] = new() {
                        ID = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationTime,
                        Balance = account.Balance,

                        Transactions = account.Transactions.Select( transaction => new TransactionDTO {
                            ID = transaction.ID,
                            Type = transaction.Type.ToString(),
                            Amount = transaction.Amount,
                            Description = transaction.Description,
                            Date = transaction.Date,
                            AccountId = transaction.AccountId
                        })
                    };
                }
            }
            return Ok("No hay cuentas creadas.");
        }
        

        [HttpGet("{id}")]
        public ActionResult<Account> Get(string id)
        {
            Account? account = accountRepository.FindByID(id);

            if(account is not null) 
            {
                return Ok( new AccountDTO {
                    ID = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationTime,
                    Balance = account.Balance,

                    Transactions = account.Transactions.Select( transaction => new TransactionDTO {
                            ID = transaction.ID,
                            Type = transaction.Type.ToString(),
                            Amount = transaction.Amount,
                            Description = transaction.Description,
                            Date = transaction.Date,
                            AccountId = transaction.AccountId
                        })
                });
            }

            return Ok("La cuenta no se ha encontrado.");
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}