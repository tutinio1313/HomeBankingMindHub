using HomeBankingMindHub.Model.DTO;
using HomeBankingMindHub.Model.Model.Loan;
using HomeBankingMindHub.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController(ILoanService loanService, IClientsLoanService clientLoansService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        IEnumerable<LoanDTO>? loanDTOs = loanService.GetAll(statusCode: out int statusCode, message: out string? message);

        if (loanDTOs is not null)
        {
            return StatusCode(statusCode, loanDTOs);
        }
        return StatusCode(statusCode, message);
    }

    [HttpPost]
    public IActionResult Post([FromBody] LoanApplicationModel model)
    {
        ClientsLoanDTO? clientsLoanDTO = clientLoansService.Post(model, User, out int statusCode, out string? message);

        if (clientsLoanDTO is not null)
        {
            return StatusCode(statusCode, clientsLoanDTO);
        }

        return StatusCode(statusCode, message);
    }
}
