using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL;
using MoneyStats.BL.Repositories;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestTableController : ControllerBase
    {
        [HttpGet("testcall")]
        public ActionResult TestCall()
        {
            return Ok(new { message = "Test call got it's response. I am that response." });
        }

        [HttpGet("get")]
        public ActionResult Get()
        {
            var repo = new BankRowRepository();
            return Ok(repo.Get());
        }
    }
}
