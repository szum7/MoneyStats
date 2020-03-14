using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoneyStats.BL.Repositories;
using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        ITransactionRepository _repo;

        public TransactionController()
        {
            _repo = new TransactionRepository(); // TODO use injection
        }

        [HttpGet("get")]
        public ActionResult Get()
        {            
            return Ok(_repo.Get());
        }
    }
}
