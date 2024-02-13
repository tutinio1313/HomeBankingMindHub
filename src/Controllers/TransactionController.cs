using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
using HomeBankingMindHub.Model.Model.Transaction;
//using HomeBankingMindHub.Model.DTO;
using System.Collections.Immutable;
using HomeBankingMindHub.Service.Interface;
using HomeBankingMindHub.Model.DTO;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransactionController(ITransactionService transactionService) : ControllerBase
{
    [HttpPost]
    public ActionResult Post([FromBody] PostTransactionModel model)
    {
        IEnumerable<TransactionDTO>? transactionsDTOs = transactionService.Post(claims : User,model : model,statusCode :out int statusCode,message : out string? message);

            if(transactionsDTOs is not null)
            {
                return StatusCode(statusCode, transactionsDTOs);
            }

            return StatusCode(statusCode, message);        
    }
}