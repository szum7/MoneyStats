using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RuleController : ControllerBase
    {
        IRuleRepository _repo;

        public RuleController(IRuleRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("getwithentities")]
        public ActionResult GetWithEntities()
        {            
            return Ok(_repo.GetWithEntities());
        }

        [HttpPost("delete"), Produces("application /json")]
        public ActionResult Delete([FromBody] int id)
        {
            return Ok(_repo.Delete(id));
        }
    }
}
