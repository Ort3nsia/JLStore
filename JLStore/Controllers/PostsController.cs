using JLStore.Domain.Models;
using JLStore.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace JLStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly TimeProvider _time;

        public PostsController(DataContext db, TimeProvider time)
        {
            _db = db;
            _time = time;
        }

        // GET: api/posts (lista pubblica)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> Get()
        {
            var items = await _db.Posts
                .AsNoTracking()
                .OrderByDescending(p => p.PublishedAt)
                .ToListAsync();

            return Ok(items);
        }

        // GET: api/posts/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetById(int id)
        {
            // Il filtro globale è attivo; se vuoi bypassarlo (admin/diagnostica), usa .IgnoreQueryFilters()
            var post = await _db.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return post is null ? NotFound() : Ok(post);
        }

        // POST: api/posts  (crea e pubblica subito in UTC, se non diversamente specificato)
        [HttpPost]
        public async Task<ActionResult<object>> Create([FromBody] CreatePostDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Title e Content sono obbligatori.");

            var nowUtc = _time.GetUtcNow();

            var post = new Post
            {
                Title = dto.Title.Trim(),
                Content = dto.Content,
                Published = dto.PublishNow,
                PublishedAt = dto.PublishNow ? nowUtc : dto.PublishedAtUtc
            };

            _db.Posts.Add(post);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = post.Id }, new { post.Id });
        }
    }

    public record CreatePostDto(
        string Title,
        string Content,
        bool PublishNow = true,
        DateTimeOffset? PublishedAtUtc = null
    );
}
