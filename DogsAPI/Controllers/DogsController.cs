using DogsAPI.DB;
using Microsoft.AspNetCore.Mvc;

namespace DogsAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class DogsController : ControllerBase
    {
        private readonly DogsContext _dbContext;

        public DogsController(DogsContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Dogs house service. Version 1.0.1");
        }

        //[HttpGet("dogs")]
        //public IActionResult GetDogs()
        //{
        //    var dogs = _dbContext.Dogs.ToList();
        //    return Ok(dogs);
        //}
    }
}