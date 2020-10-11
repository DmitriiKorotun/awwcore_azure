using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace awwcore_azure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameGenresController : ControllerBase
    {
        private readonly GameReviewsContext _context;

        public GameGenresController(GameReviewsContext context)
        {
            _context = context;
        }

        // GET: api/GameGenres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameGenre>>> GetGameGenres()
        {
            return await _context.GameGenres.Include(gg => gg.Game)
                .Include(gg => gg.Genre)
                .ToListAsync();
        }

        // GET: api/GameGenres/5
        [HttpGet("{gameId}")]
        public async Task<ActionResult<IEnumerable<GameGenre>>> GetGameGenres(int gameId)
        {
            var GameGenre = await _context.GameGenres.Where(gg => gg.GameId == gameId)
                .Include(gg => gg.Game)
                .Include(gg => gg.Genre)
                .ToListAsync();

            if (GameGenre == null || GameGenre.Count < 1)
            {
                return NotFound();
            }

            return GameGenre;
        }

        // PUT: api/GameGenres/5,2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameGenre(int gameId, int genreId, GameGenre GameGenre)
        {
            if (gameId != GameGenre.GameId || genreId != GameGenre.GenreId)
            {
                return BadRequest();
            }

            _context.Entry(GameGenre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameGenreExists(gameId, genreId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameGenres
        [HttpPost]
        public async Task<ActionResult<GameGenre>> PostGameGenre(GameGenre GameGenre)
        {
            _context.GameGenres.Add(GameGenre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameGenres", new { gameId = GameGenre.GameId }, GameGenre);
        }

        // DELETE: api/GameGenres/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameGenre>> DeleteGameGenre(int gameId, int genreId)
        {
            var GameGenre = await _context.GameGenres.Where(gg => gg.GameId == gameId && gg.GenreId == genreId)
                .FirstOrDefaultAsync();

            if (GameGenre == null)
            {
                return NotFound();
            }

            _context.GameGenres.Remove(GameGenre);
            await _context.SaveChangesAsync();

            return GameGenre;
        }

        private bool GameGenreExists(int gameId, int genreId)
        {
            return _context.GameGenres.Any(e => e.GameId == gameId && e.GenreId == genreId);
        }
    }
}
