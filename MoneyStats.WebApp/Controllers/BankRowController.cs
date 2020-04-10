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
    public class BankRowController : ControllerBase
    {
        IBankRowRepository _repo;

        public BankRowController(IBankRowRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("get")]
        public ActionResult Get()
        {            
            return Ok(_repo.Get());
        }
    }
}
