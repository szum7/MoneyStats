using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;
using MoneyStats.DAL.Models;
using System.Collections.Generic;

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


        #region Get
        [HttpGet("getwithentities")]
        public ActionResult GetWithEntities()
        {            
            return Ok(_repo.GetWithEntities());
        }
        #endregion


        #region Post
        [HttpPost("delete"), Produces("application /json")]
        public ActionResult Delete([FromBody] int id)
        {
            return Ok(_repo.Delete(id));
        }

        [HttpPost("saverules"), Produces("application /json")]
        public ActionResult SaveRules([FromBody] List<Rule> rules)
        {
            return Ok(_repo.SaveRules(rules));
        }

        [HttpPost("save"), Produces("application /json")]
        public ActionResult Save([FromBody] Rule rule)
        {
            return Ok(_repo.Save(rule));
        }
        #endregion
    }
}
