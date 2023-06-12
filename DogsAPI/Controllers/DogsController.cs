using DogsAPI.DB;
using DogsAPI.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog(Dog dog)
        {
            if (ModelState.IsValid)
            {
                var existingDog = await _dbContext.Dogs.FirstOrDefaultAsync(d => d.Name == dog.Name);
                if (existingDog != null)
                {
                    return Conflict("A dog with the same name already exists.");
                }

                if (dog.TailLength < 0)
                {
                    return BadRequest("Tail length cannot be a negative number.");
                }

                if (dog.Weight < 0)
                {
                    return BadRequest("Tail weight cannot be a negative number.");
                }

                _dbContext.Dogs.Add(dog);
                await _dbContext.SaveChangesAsync();

                return Ok("Dog created successfully.");
            }

            return BadRequest("Invalid dog data.");
        }

        [HttpGet("dogs")]
        public IActionResult GetDogs()
        {
            var dogs = _dbContext.Dogs;
            return Ok(dogs);
        }
    }
}