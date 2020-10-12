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
    public class GamePlatformsController : ControllerBase
    {
        private readonly GameReviewsContext _context;

        public GamePlatformsController(GameReviewsContext context)
        {
            _context = context;
        }

        // GET: api/GamePlatforms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GamePlatform>>> GetGamePlatforms()
        {
            return await _context.GamePlatforms.Include(gp => gp.Game)
                .Include(gp => gp.Platform)
                .ToListAsync();
        }

        // GET: api/GamePlatforms/5
        [HttpGet("{gameId}")]
        public async Task<ActionResult<IEnumerable<GamePlatform>>> GetGamePlatforms(int gameId)
        {
            var GamePlatform = await _context.GamePlatforms.Where(gg => gg.GameId == gameId)
                .Include(gp => gp.Game)
                .Include(gp => gp.Platform)
                .ToListAsync();

            if (GamePlatform == null || GamePlatform.Count < 1)
            {
                return NotFound();
            }

            return GamePlatform;
        }

        // PUT: api/GamePlatforms/5&2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGamePlatform(int gameId, int platformId, GamePlatform gamePlatform)
        {
            if (gameId != gamePlatform.GameId || !_context.Platforms.Any(p => p.ID == gamePlatform.PlatformId))
            {
                return BadRequest();
            }

            _context.Entry(gamePlatform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GamePlatformExists(gameId, platformId))
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

        // POST: api/GamePlatforms
        [HttpPost]
        public async Task<ActionResult<GamePlatform>> PostGamePlatform(GamePlatform gamePlatform)
        {
            if (!_context.Platforms.Any(p => p.ID == gamePlatform.PlatformId)
                || !_context.Games.Any(g => g.ID == gamePlatform.GameId))
            {
                return BadRequest();
            }

            _context.GamePlatforms.Add(gamePlatform);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGamePlatforms", new { gameId = gamePlatform.GameId }, gamePlatform);
        }

        // DELETE: api/GamePlatforms/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GamePlatform>> DeleteGamePlatform(int gameId, int platformId)
        {
            var GamePlatform = await _context.GamePlatforms.Where(gg => gg.GameId == gameId && gg.PlatformId == platformId)
                .FirstOrDefaultAsync();

            if (GamePlatform == null)
            {
                return NotFound();
            }

            _context.GamePlatforms.Remove(GamePlatform);
            await _context.SaveChangesAsync();

            return GamePlatform;
        }

        private bool GamePlatformExists(int gameId, int platformId)
        {
            return _context.GamePlatforms.Any(e => e.GameId == gameId && e.PlatformId == platformId);
        }
    }
}
