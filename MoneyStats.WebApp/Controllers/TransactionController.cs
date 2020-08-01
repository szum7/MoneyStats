using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        ITransactionRepository _repo;

        public TransactionController(ITransactionRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("getTransactionProperties")]
        public ActionResult GetTransactionProperties()
        {
            return Ok(_repo.GetTransactionRulableProperties());
        }
    }
}
