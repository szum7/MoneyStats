using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyStats.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;

namespace MoneyStats.WebApp.Controllers
{
    public class GetSuggestedsWrap
    {
        public List<Rule> rules { get; set; }
        public List<BankRow> bankRows { get; set; }
    }

    /// <summary>
    /// The main functionality is to save the transactions and their connections into the database.
    /// They are called "generated", because they are originally/mainly generated from BankRows.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GeneratedTransactionController : ControllerBase
    {
        IGeneratedTransactionService _repo;

        public GeneratedTransactionController(IGeneratedTransactionService repo)
        {
            _repo = repo;
        }

        [HttpPost("getsuggesteds"), Produces("application/json")]
        public ActionResult GetSuggesteds([FromBody] GetSuggestedsWrap data)
        {
            return Ok(_repo.Get(data.rules, data.bankRows));
        }
    }
}
