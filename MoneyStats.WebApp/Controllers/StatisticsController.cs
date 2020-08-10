using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using System;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        IStatisticsService _repo;

        public StatisticsController(IStatisticsService repo)
        {
            _repo = repo;
        }

        [HttpGet("getbasicmonthlybarchart")]
        public ActionResult GetBasicMonthlyBarchart(DateTime from, DateTime to)
        {
            return Ok(_repo.Get(from, to));
        }
    }
}