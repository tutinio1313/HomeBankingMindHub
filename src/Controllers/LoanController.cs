using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Loan;
using HomeBankingMindHub.Service.Interface;

namespace HomeBankingMindHub.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController(ILoanService loanService, IClientsLoanService clientLoansService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        IEnumerable<LoanDTO>? loanDTOs = loanService.GetAll( statusCode: out int statusCode, message : out string? message);

        if(loanDTOs is not null)
        {
            return StatusCode(statusCode, loanDTOs);
        }
        return StatusCode(statusCode, message);
    }

    [HttpPost]
    public IActionResult Post([FromBody] LoanApplicationModel model)
    {
        var Email = User.FindFirstValue(ClaimTypes.Email);
        if(Email is not null)
        {
            ClientsLoanDTO? clientsLoanDTO = clientLoansService.Post(model, Email, out int statusCode, out string? message);

            if(clientsLoanDTO is not null)
            {
                return StatusCode(statusCode, clientsLoanDTO);
            }

            return StatusCode(statusCode, message); 
        }
        return StatusCode(401,"No tienes los permisos suficientes.");
    }
}
