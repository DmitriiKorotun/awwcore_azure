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

            int newItemsCount = game.GameGenres.Count;

            _context.Games
                .Include(x => x.GameGenres)
                .FirstOrDefault(x => x.ID == game.ID);

            int oldItemsCount = game.GameGenres.Count - newItemsCount;

            for(int i = newItemsCount; i < newItemsCount + oldItemsCount; ++i)
                _context.GameGenres.Remove(game.GameGenres[i]);

            for (int i = 0; i < newItemsCount; ++i)
                _context.GameGenres.Add(game.GameGenres[i]);

            //_context.TryUpdateManyToMany(oldGame.GameGenres, game.GameGenres, x => x.GenreId);

            //foreach (GameGenre gameGenre in _context.GameGenres.Where(gg => gg.GameId == game.ID).ToList())
            //{
            //    _context.Entry(gameGenre).State = EntityState.Deleted;
            //}

            //foreach (GameGenre gameGenre in game.GameGenres)
            //{
            //    if (_context.GameGenres.Any(gg => gg.GameId == gameGenre.GameId && gg.GenreId == gameGenre.GenreId))
            //        _context.Entry(gameGenre).State = EntityState.Modified;
            //    else
            //        _context.Entry(gameGenre).State = EntityState.Added;
            //}

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
