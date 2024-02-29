using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Transaction;
using HomeBankingMindHub.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ClientService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public ActionResult Post([FromBody] PostTransactionModel model)
    {
        string? Email = User.FindFirstValue("Client");

        if (!Email.IsNullOrEmpty())
        {
            IEnumerable<TransactionDTO>? transactionsDTOs = transactionService.Post(email: Email, model: model, statusCode: out int statusCode, message: out string? message);

            if (transactionsDTOs is not null)
            {
                return StatusCode(statusCode, transactionsDTOs);
            }

            return StatusCode(statusCode, message);
        }
        return StatusCode(401, "No tienes los permisos necesarios para realizar esta acción.");
    }
}