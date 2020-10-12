using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using awwcore_azure.Database.Handlers;

namespace awwcore_azure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameReviewsContext _context;

        public GamesController(GameReviewsContext context)
        {
            _context = context;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var ls = await _context.Games
                .Include(g => g.Publisher)
                .Include(g => g.Developer)
                .Include(g => g.GameGenres)
                .Include(g => g.GamePlatforms)
                .ToListAsync();

            return ls;
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _context.Games.Where(g => g.ID == id)
                .Include(g => g.Publisher)
                .Include(g => g.Developer)
                .Include(g => g.GameGenres)
                //.ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms)
                //.ThenInclude(gp => gp.Platform)
                .FirstOrDefaultAsync();

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.ID || !_context.Publishers.Any(p => p.ID == game.PublisherId)
    || !_context.Developers.Any(d => d.ID == game.DeveloperId))
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                _context.UpdateGameGenres(game);
                _context.UpdateGamePlatforms(game);
            }
            catch(ArgumentException)
            {
                return BadRequest();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
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

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            if (!_context.Publishers.Any(p => p.ID == game.PublisherId)
|| !_context.Developers.Any(d => d.ID == game.DeveloperId))
            {
                return BadRequest();
            }

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.ID }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return game;
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.ID == id);
        }
    }
}
