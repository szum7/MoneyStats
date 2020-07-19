using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL;
using MoneyStats.BL.Repositories;
using System.Collections.Generic;

namespace MoneyStats.WebApp.Controllers
{
    public class MyPostWrap
    {
        public int id { get; set; }
        public List<string> names { get; set; }
    }

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

        [HttpPost("mypost"), Produces("application/json")]
        public ActionResult MyPost(MyPostWrap data)
        {
            try
            {
                var name = data.names[0];
            }
            catch (System.Exception)
            {
            }

            return Ok(new { message = "ok" });
        }
    }
}
