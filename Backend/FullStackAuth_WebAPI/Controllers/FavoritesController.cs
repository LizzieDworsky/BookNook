using FullStackAuth_WebAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        //[HttpGet("{all}")]
        //public IActionResult Get()
        //{
        //    var favorites = _context.Favorites.ToList();
        //    return Ok(favorites);
        //}

        //GET api/<FavoritesController>
        /// <summary>
        /// Retrieves all favorites of the currently authenticated user.
        /// </summary>
        /// <returns>A list of all favorites of the authenticated user, Unauthorized if the user id is not found, or an empty list if there are no favorites for this user.</returns>
        [HttpGet, Authorize]
        public IActionResult Get()
        {
            string userId = User.FindFirstValue("id");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var favorites = _context.Favorites.Where(f => f.UserId == userId).ToList();
            return Ok(favorites);
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
