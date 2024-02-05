using Microsoft.AspNetCore.Mvc;

using HomeBankingMindHub.Database.Repository;
using HomeBankingMindHub.Model.Entity;
//using HomeBankingMindHub.Model.Model.Client;
//using HomeBankingMindHub.Model.DTO;
using System.Collections.Immutable;

namespace Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
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