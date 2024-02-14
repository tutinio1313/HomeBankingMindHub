using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<AccountDTO>> Get()
        {
            var accounts = accountService.GetAll(
                out int statusCode,
                 out string? message);
            
            if(accounts is not null){
                return StatusCode(statusCode, accounts);
            }

            return StatusCode(statusCode, message);
        }
        

        [HttpGet("{id}")]
        public ActionResult<AccountDTO> Get(string id)
        {
            var account = accountService.GetByID(id : id,
             out int statusCode,
             out string? message);

            if(account is not null) {
                return StatusCode(statusCode, account);
            }

            return StatusCode(statusCode, message);
        }
    }
}