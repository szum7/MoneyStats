using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyStats.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using MoneyStats.BL.Modules;

namespace MoneyStats.WebApp.Controllers
{
    public class GetSuggestedsWrap
    {
        public List<Rule> rules { get; set; }
        public List<BankRow> bankRows { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class GeneratedTransactionController : ControllerBase
    {
        IGeneratedTransactionRepository _repo;

        public GeneratedTransactionController(IGeneratedTransactionRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("getgenerated"), Produces("application/json")]
        public ActionResult GetGenerated([FromBody] GetSuggestedsWrap data)
        {
            return Ok(_repo.Generate(data.rules, data.bankRows));
        }

        [HttpPost("save"), Produces("application /json")]
        public ActionResult Save([FromBody] List<GeneratedTransaction> generatedTransactions)
        {
            return Ok(_repo.SaveAll(generatedTransactions));
        }
    }
}
