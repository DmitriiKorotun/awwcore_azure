using awwcore_azure.Controllers;
using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace awwcore_azure.Tests.ControllersTests
{
    public class GamesControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetGames_Void_TaskActionResultContainsIEnumerableOfGame()
        {
            // Arrange
            List<Game> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                IEnumerable<Game> games = (await gamesController.GetGames()).Value;

                // Assert
                Assert.Equal(expectedData.Count, games.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], games.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetGame_ExistingId_TaskActionResultContainsGame()
        {
            // Arrange
            const int gameId = 3;

            Game expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                Game game = (await gamesController.GetGame(gameId)).Value;
                ActionResult result = (await gamesController.GetGame(gameId)).Result;

                // Assert              
                Assert.True(AreEqual(expectedGame, game));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGame_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int gameId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.GetGame(gameId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGame_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int gameId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.GetGame(gameId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGame_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int gameId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.GetGame(gameId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithNameChanged_NoContentResult()
        {
            // Arrange
            const int gameId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.Name = "newName";
                expectedGame.Name = "newName";

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithDescriptionChanged_NoContentResult()
        {
            // Arrange
            const int gameId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.Description = "newDescription";
                expectedGame.Description = "newDescription";

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithAnnouncementDateChanged_NoContentResult()
        {
            // Arrange
            const int gameId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.AnnouncementDate = DateTime.Parse("14-10-2020");
                expectedGame.AnnouncementDate = DateTime.Parse("14-10-2020");

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithReleaseDateChanged_NoContentResult()
        {
            // Arrange
            const int gameId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.ReleaseDate = DateTime.Parse("14-10-2020");
                expectedGame.ReleaseDate = DateTime.Parse("14-10-2020");

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithDeveloperIdChangedToExisting_NoContentResultDeveloperIdChanged()
        {
            // Arrange
            const int gameId = 3, developerId = 7;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.DeveloperId = developerId;
                expectedGame.DeveloperId = developerId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithDeveloperIdChangedToNonexisting_NoContentResultDeveloperIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, developerId = 20;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.DeveloperId = developerId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithDeveloperIdChangedToZero_NoContentResultDeveloperIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, developerId = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.DeveloperId = developerId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithDeveloperIdChangedToNegative_NoContentResultDeveloperIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, developerId = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.DeveloperId = developerId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }


        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithPublisherIdChangedToExisting_NoContentResultPublisherIdChanged()
        {
            // Arrange
            const int gameId = 3, publisherId = 7;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.PublisherId = publisherId;
                expectedGame.PublisherId = publisherId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGame, actualGame));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithPublisherIdChangedToNonexisting_NoContentResultPublisherIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, publisherId = 20;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.PublisherId = publisherId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithPublisherIdChangedToZero_NoContentResultPublisherIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, publisherId = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.PublisherId = publisherId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithPublisherIdChangedToNegative_NoContentResultPublisherIdDoesntChanged()
        {
            // Arrange
            const int gameId = 3, publisherId = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGame = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.PublisherId = publisherId;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int gameId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.ID = idChanged;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int gameId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.ID = idChanged;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int gameId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.ID = idChanged;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGame_ExistingIdCorrectGameWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int gameId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToUpdate = GetFakeList().Where(d => d.ID == gameId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                gameToUpdate.ID = idChanged;

                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PutGame(gameId, gameToUpdate));

                Game actualGame = await context.Games.Where(d => d.ID == gameId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.True(context.Games.Contains(gameToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.True(context.Games.Contains(gameToCreate));
                Assert.True(gameToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                    GamesController gamesController = new GamesController(context);
                    var result = (await gamesController.PostGame(gameToCreate)).Result;

                    // Assert
                    Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.True(gameToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptNonexistingPublisherIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 20,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptZeroPublisherIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 0,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptNegativePublisherIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = -1,
                DeveloperId = 2,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptNonexistingDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 2,
                DeveloperId = 20,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptZeroDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 2,
                DeveloperId = 0,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGame_CorrectGameWithCorrectValuesExceptNegativeDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Game gameToCreate = new Game()
            {
                Name = "NewGame",
                Description = "NewDescription",
                PublisherId = 2,
                DeveloperId = -1,
                AnnouncementDate = DateTime.Parse("10-05-2015"),
                ReleaseDate = DateTime.Parse("27-12-2018")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var result = (await gamesController.PostGame(gameToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteGame_ExistingId_TaskActionResultContainsDeletedGame()
        {
            // Arrange
            const int gameIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Game expectedGame = GetFakeList().Where(d => d.ID == gameIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var actionResult = (await gamesController.DeleteGame(gameIdToDelete));
                var result = actionResult.Result;
                Game actualGame = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Games.Find(gameIdToDelete) == null);
                Assert.True(AreEqual(expectedGame, actualGame));
            }
        }

        [Fact]
        public async Task DeleteGame_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int gameIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Game expectedGame = GetFakeList().Where(d => d.ID == gameIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var actionResult = (await gamesController.DeleteGame(gameIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteGame_ZeroId_NotFoundResult()
        {
            // Arrange
            const int gameIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Game expectedGame = GetFakeList().Where(d => d.ID == gameIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var actionResult = (await gamesController.DeleteGame(gameIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteGame_NegativeId_NotFoundResult()
        {
            // Arrange
            const int gameIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Game expectedGame = GetFakeList().Where(d => d.ID == gameIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GamesController gamesController = new GamesController(context);
                var actionResult = (await gamesController.DeleteGame(gameIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Game> GetFakeList()
        {
            var data = new List<Game>
            {
                new Game { Name = "BBB", ID = 2, Description = "DdD",
                    AnnouncementDate = DateTime.Parse("13-09-2019"), ReleaseDate = DateTime.Parse("13-09-2019"),
                    DeveloperId = 2, PublisherId = 3,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>()},

                new Game { Name = "ZZZ", ID = 3, Description = "OOo",
                    AnnouncementDate = DateTime.Parse("15-11-2017") , ReleaseDate = DateTime.Parse("03-03-2021"),
                    DeveloperId = 4, PublisherId = 5,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>() },

                new Game { Name = "AAA", ID = 4, Description = "-><",
                    AnnouncementDate = DateTime.Parse("01-01-2018") , ReleaseDate = DateTime.Parse("24-12-2022"),
                    DeveloperId = 6, PublisherId = 7,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>() }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "GamesDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Games.Add(new Game
                {
                    Name = "BBB",
                    ID = 2,
                    Description = "DdD",
                    AnnouncementDate = DateTime.Parse("13-09-2019"),
                    ReleaseDate = DateTime.Parse("13-09-2019"),
                    DeveloperId = 2,
                    PublisherId = 3,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>()
                });
                context.Games.Add(new Game {
                    Name = "ZZZ",
                    ID = 3,
                    Description = "OOo",
                    AnnouncementDate = DateTime.Parse("15-11-2017"),
                    ReleaseDate = DateTime.Parse("03-03-2021"),
                    DeveloperId = 4,
                    PublisherId = 5,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>()
                });
                context.Games.Add(new Game {
                    Name = "AAA",
                    ID = 4,
                    Description = "-><",
                    AnnouncementDate = DateTime.Parse("01-01-2018"),
                    ReleaseDate = DateTime.Parse("24-12-2022"),
                    DeveloperId = 6,
                    PublisherId = 7,
                    GameGenres = new List<GameGenre>(),
                    GamePlatforms = new List<GamePlatform>()
                });

                for(int i = 0; i < 10; ++i)
                {
                    context.Developers.Add(new Developer(){ Name = i.ToString(), Website = "Website" + i });
                    context.Publishers.Add(new Publisher() { Name = i.ToString(), Website = "Website" + i });
                }
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Game> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "GamesDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Game game in values)
                    context.Games.Add(game);

                for (int i = 0; i < 10; ++i)
                {
                    context.Developers.Add(new Developer() { Name = i.ToString(), Website = "Website" + i });
                    context.Publishers.Add(new Publisher() { Name = i.ToString(), Website = "Website" + i });
                }

                context.SaveChanges();
            }
        }

        private bool AreEqual(Game expected, Game actual)
        {
            bool areEqual = true;

            if ((expected == null && actual != null) || (expected != null && actual == null))
                areEqual = false;
            else if (expected.ID != actual.ID || expected.Name != actual.Name || expected.Description != actual.Description
                || !expected.AnnouncementDate.Equals(actual.AnnouncementDate) || !expected.ReleaseDate.Equals(actual.ReleaseDate)
                || expected.PublisherId != actual.PublisherId || expected.DeveloperId != actual.DeveloperId)
                areEqual = false;

            return areEqual;
        }
    }
}
