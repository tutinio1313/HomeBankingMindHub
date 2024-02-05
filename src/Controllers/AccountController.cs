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
        public ActionResult<IEnumerable<Account>> Get()
        {
            var accounts = accountRepository.GetAllAccounts();

            if(accounts is not null) {
                return accounts.ToArray();
            }
            return Ok("No hay cuentas creadas.");
        }
        

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
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