using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyStats.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.BL.Modules;

namespace MoneyStats.WebApp.Controllers
{
    /// <summary>
    /// The main functionality is to run the rule evaluation program, and get suggested transactions.
    /// These suggestions then can be displayed and edited on the UI.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SuggestedTransactionController : ControllerBase
    {
        ISuggestedTransactionService _service;

        public SuggestedTransactionController(ISuggestedTransactionService service)
        {
            _service = service;
        }

        [HttpPost("save"), Produces("application /json")]
        public ActionResult Save([FromBody] List<SuggestedTransaction> suggestedTransactions)
        {
            return Ok(_service.SaveAll(suggestedTransactions));
        }
    }
}
