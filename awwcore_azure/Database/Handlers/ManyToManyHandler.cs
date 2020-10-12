using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Handlers
{
    public static class ManyToManyHandler
    {
        public static void UpdateGameGenres(this GameReviewsContext context, Game game)
        {
            foreach (GameGenre gameGenre in game.GameGenres)
                if (gameGenre.GameId != game.ID)
                    throw new ArgumentException("Genre gameId doesnt match with game id");

            int newItemsCount = game.GameGenres.Count;

            context.Games
                .Include(x => x.GameGenres)
                .FirstOrDefault(x => x.ID == game.ID);

            int oldItemsCount = game.GameGenres.Count - newItemsCount;

            for (int i = newItemsCount; i < newItemsCount + oldItemsCount; ++i)
                context.GameGenres.Remove(game.GameGenres[i]);

            for (int i = 0; i < newItemsCount; ++i)
            {
                var existingGameGenre = context.GameGenres.Where(gg => gg.GameId == game.GameGenres[i].GameId && gg.GenreId == game.GameGenres[i].GenreId).FirstOrDefault();

                if(existingGameGenre != null && context.Entry(existingGameGenre).State != EntityState.Deleted)
                    throw new ArgumentException("Found duplicate of game genre");

                context.GameGenres.Add(game.GameGenres[i]);
            }
        }

        public static void UpdateGamePlatforms(this GameReviewsContext context, Game game)
        {
            foreach (GamePlatform gamePlatform in game.GamePlatforms)
                if (gamePlatform.GameId != game.ID)
                    throw new ArgumentException("Platform gameId doesnt match with game id");

            int newItemsCount = game.GamePlatforms.Count;

            context.Games
                .Include(x => x.GamePlatforms)
                .FirstOrDefault(x => x.ID == game.ID);

            int oldItemsCount = game.GamePlatforms.Count - newItemsCount;

            for (int i = newItemsCount; i < newItemsCount + oldItemsCount; ++i)
                context.GamePlatforms.Remove(game.GamePlatforms[i]);

            for (int i = 0; i < newItemsCount; ++i)
            {
                var existingGamePlatform = context.GamePlatforms.Where(gg => gg.GameId == game.GamePlatforms[i].GameId && gg.PlatformId == game.GamePlatforms[i].PlatformId).FirstOrDefault();

                if (existingGamePlatform != null && context.Entry(existingGamePlatform).State != EntityState.Deleted)
                    throw new ArgumentException("Found duplicate of game platform");

                context.GamePlatforms.Add(game.GamePlatforms[i]);
            }
        }
    }
}
