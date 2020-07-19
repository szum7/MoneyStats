using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

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

        [HttpPost("save"), Produces("application /json")]
        public ActionResult Save(List<BankRow> data)
        {
            return Ok(_repo.InsertRange(data));
        }
    }
}
