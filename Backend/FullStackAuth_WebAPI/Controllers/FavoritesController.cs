using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.Models;
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

        /// <summary>
        /// Creates a new favorite for the currently authenticated user.
        /// </summary>
        /// <param name="newFavorite">The favorite to create.</param>
        /// <returns>
        /// The created favorite with a 201 status code if the operation is successful,
        /// BadRequest with ModelState if the model is not valid,
        /// Unauthorized if the user's id is not found,
        /// or a 500 status code with exception details if a server error occurred.
        /// </returns>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Favorite newFavorite)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                newFavorite.UserId = userId;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Favorites.Add(newFavorite);
                _context.SaveChanges();
                return StatusCode(201, newFavorite);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        /// <summary>
        /// Deletes specified favorite by pk.
        /// </summary>
        /// <param name="id">Primary Key of Favorite</param>
        /// <returns>
        /// Returns NoContent if the favorite was deleted, NotFound if the favorite was not found, 
        /// Unauthorized if the user is not authenticated or if the favorite does not belong to the authenticated user.
        /// </returns>
        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            var favorite = _context.Favorites.Where(f => f.Id == id).SingleOrDefault();
            if (favorite == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue("id");
            if (string.IsNullOrEmpty(userId) || favorite.UserId != userId)
            {
                return Unauthorized();
            }

            _context.Favorites.Remove(favorite);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
