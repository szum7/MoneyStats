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

        [HttpGet("get")]
        public ActionResult Get()
        {            
            return Ok(_repo.GetWithEntities());
        }
    }
}
