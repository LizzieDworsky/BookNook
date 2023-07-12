using FullStackAuth_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public FavoritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<FavoritesController>/all
        /// <summary>
        /// Testing only endpoint. Retrieves all favorites.
        /// </summary>
        /// <returns>A list of all favorites or empty list if there are no favorites.</returns>
        [HttpGet("{all}")]
        public IActionResult Get()
        {
            var favorites = _context.Favorites.ToList();
            return Ok(favorites);
        }

        //GET api/<FavoritesController>
        [HttpGet, Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FavoritesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FavoritesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FavoritesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
