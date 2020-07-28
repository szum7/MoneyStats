using Microsoft.AspNetCore.Mvc;
using MoneyStats.BL.Interfaces;

namespace MoneyStats.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : ControllerBase
    {
        ITagRepository _repo;

        public TagController(ITagRepository repo)
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
