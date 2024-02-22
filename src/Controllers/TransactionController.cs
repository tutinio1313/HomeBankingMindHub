using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Model.Model.Transaction;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Model.DTO;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public ActionResult Post([FromBody] PostTransactionModel model)
    {
        string? Email = User.FindFirstValue(ClaimTypes.Email);

        if(!Email.IsNullOrEmpty())
        {   
            #pragma warning disable
            IEnumerable<TransactionDTO>? transactionsDTOs = transactionService.Post(email : Email,model : model,statusCode :out int statusCode,message : out string? message);
            #pragma warning restore
                if(transactionsDTOs is not null)
                {
                    return StatusCode(statusCode, transactionsDTOs);
                }

                return StatusCode(statusCode, message);        
        }
        return StatusCode(401, "No tienes los permisos necesarios para realizar esta acción.");
    }
}