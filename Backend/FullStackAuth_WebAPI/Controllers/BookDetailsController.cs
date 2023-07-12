using FullStackAuth_WebAPI.Data;
using FullStackAuth_WebAPI.DataTransferObjects;
using FullStackAuth_WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FullStackAuth_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDetailsController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public BookDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{bookId}")]
        public IActionResult Get(string bookId)
        {
            var reviews = _context.Reviews.Where(r => r.BookId == bookId).ToList();
            if (reviews.Count == 0)
            {
                var emptyBookDetailsDto = new BookDetailsDto();
                return Ok(emptyBookDetailsDto);
            }
            bool isFavorite = false;
            string userId = User.FindFirstValue("id");
            if (!string.IsNullOrEmpty(userId))
            {
                var favorites = _context.Favorites.Where(f => f.BookId == bookId && f.UserId == userId).ToList();
                if (favorites.Count > 0)
                {
                    isFavorite = true;
                }
            }

            var bookDetailsDto = new BookDetailsDto
            {
                Reviews = reviews.Select(r => new ReviewWithUserDto
                {
                    Id = r.Id,
                    BookId = r.BookId,
                    Text = r.Text,
                    Rating = r.Rating,
                    User = _context.Users.Where(u => u.Id == r.UserId).Select(u => new UserForDisplayDto  //refactor for less API calls
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName
                    }).SingleOrDefault()
                }).ToList(),
                AverageRating = reviews.Select(r => r.Rating).Average(),
                IsFavorite = isFavorite
            };
            return Ok(bookDetailsDto);
        }

    }
}
