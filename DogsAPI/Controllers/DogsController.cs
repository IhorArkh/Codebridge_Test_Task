using DogsAPI.DB;
using DogsAPI.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        [HttpGet("dogs")]
        public ActionResult GetDogs(string? attribute, string? order, int pageNumber = 0, int dogsPerPage = 0)
        {
            IQueryable<Dog> query = _dbContext.Dogs;

            //Sorting
            if (attribute != null && order != null)
            {
                switch (attribute.ToLower())
                {
                    case "name":
                        query = (order.ToLower() == "desc") ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name);
                        break;
                    case "color":
                        query = (order.ToLower() == "desc") ? query.OrderByDescending(d => d.Color) : query.OrderBy(d => d.Color);
                        break;
                    case "tail_length":
                        query = (order.ToLower() == "desc") ? query.OrderByDescending(d => d.TailLength) : query.OrderBy(d => d.TailLength);
                        break;
                    case "weight":
                        query = (order.ToLower() == "desc") ? query.OrderByDescending(d => d.Weight) : query.OrderBy(d => d.Weight);
                        break;
                    default:
                        return BadRequest("Invalid attribute for sorting dogs.");
                }
            }
            
            //Pagination
            if (pageNumber != 0 && dogsPerPage != 0)
            {
                var dogs = query.Skip((pageNumber - 1) * dogsPerPage).Take(dogsPerPage).ToList();
                return Ok(dogs);
            }
            
            return Ok(query);
        }

        [HttpPost("dog")]
        public async Task<IActionResult> CreateDog(Dog dog)
        {
            bool isAnyFieldNull = typeof(Dog).GetProperties().Any(property => property.GetValue(dog) == null);

            if (ModelState.IsValid && !isAnyFieldNull)
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
                    return BadRequest("Weight cannot be a negative number.");
                }

                await _dbContext.Dogs.AddAsync(dog);
                await _dbContext.SaveChangesAsync();

                return Ok("Dog created successfully.");
            }

            return BadRequest("Invalid dog data.");
        }
    }
}