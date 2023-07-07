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
    public class ReviewsController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<ReviewsController>
        /// <summary>
        /// Testing only endpoint. Retrieves all reviews.
        /// </summary>
        /// <returns>A list of all reviews or empty list if there are no reviews.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            var reviews = _context.Reviews.ToList();
            return Ok(reviews);
        }


        // POST api/<ReviewsController>
        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="newReview">The review to create.</param>
        /// <returns>
        /// The created review with a 201 status code if the operation is successful,
        /// BadRequest with ModelState if the model is not valid,
        /// Unauthorized if the user's id is not found,
        /// or a 500 status code with exception details if a server error occurred.
        /// </returns>
        [HttpPost, Authorize]
        public IActionResult Post([FromBody] Review newReview)
        {
            try
            {
                string userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized();
                }
                newReview.UserId = userId;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _context.Reviews.Add(newReview);
                _context.SaveChanges();
                return StatusCode(201, newReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Testing only endpoint. Deletes specified review by pk.
        /// </summary>
        /// <param name="id">Primary Key of Review</param>
        /// <returns>Returns NoContent if the review was deleted, NotFound if the review was not found</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var review = _context.Reviews.Where(r => r.Id == id).SingleOrDefault();
            if (review == null)
            {
                return NotFound();
            }
            _context.Reviews.Remove(review);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
