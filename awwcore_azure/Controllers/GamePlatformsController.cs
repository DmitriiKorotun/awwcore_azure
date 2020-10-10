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
            return await _context.GamePlatforms.ToListAsync();
        }

        // GET: api/GamePlatforms/5
        [HttpGet("{gameId}")]
        public async Task<ActionResult<IEnumerable<GamePlatform>>> GetGamePlatforms(int gameId)
        {
            var GamePlatform = await _context.GamePlatforms.Where(gg => gg.GameId == gameId)
                .ToListAsync();

            if (GamePlatform == null || GamePlatform.Count < 1)
            {
                return NotFound();
            }

            return GamePlatform;
        }

        // PUT: api/GamePlatforms/5,2
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGamePlatform(int gameId, int platformId, GamePlatform GamePlatform)
        {
            if (gameId != GamePlatform.GameId || platformId != GamePlatform.PlatformId)
            {
                return BadRequest();
            }

            _context.Entry(GamePlatform).State = EntityState.Modified;

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
        public async Task<ActionResult<GamePlatform>> PostGamePlatform(GamePlatform GamePlatform)
        {
            _context.GamePlatforms.Add(GamePlatform);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGamePlatforms", new { gameId = GamePlatform.GameId }, GamePlatform);
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
