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
    public class GenresControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetGenres_Void_TaskActionResultContainsIEnumerableOfGenre()
        {
            // Arrange
            List<Genre> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                IEnumerable<Genre> genres = (await genresController.GetGenres()).Value;

                // Assert
                Assert.Equal(expectedData.Count, genres.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], genres.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetGenre_ExistingId_TaskActionResultContainsGenre()
        {
            // Arrange
            const int genreId = 3;

            Genre expectedGenre = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                Genre genre = (await genresController.GetGenre(genreId)).Value;
                ActionResult result = (await genresController.GetGenre(genreId)).Result;

                // Assert              
                Assert.True(AreEqual(expectedGenre, genre));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGenre_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int genreId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.GetGenre(genreId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGenre_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int genreId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.GetGenre(genreId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetGenre_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int genreId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.GetGenre(genreId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutGenre_ExistingIdCorrectGenreWithNameChanged_NoContentResult()
        {
            // Arrange
            const int genreId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToUpdate = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedGenre = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                genreToUpdate.Name = "newName";
                expectedGenre.Name = "newName";

                GenresController genresController = new GenresController(context);
                var result = (await genresController.PutGenre(genreId, genreToUpdate));

                Genre actualGenre = await context.Genres.Where(d => d.ID == genreId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedGenre, actualGenre));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutGenre_ExistingIdCorrectGenreWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int genreId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToUpdate = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                genreToUpdate.ID = idChanged;

                GenresController genresController = new GenresController(context);
                var result = (await genresController.PutGenre(genreId, genreToUpdate));

                Genre actualGenre = await context.Genres.Where(d => d.ID == genreId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGenre_ExistingIdCorrectGenreWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int genreId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToUpdate = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                genreToUpdate.ID = idChanged;

                GenresController genresController = new GenresController(context);
                var result = (await genresController.PutGenre(genreId, genreToUpdate));

                Genre actualGenre = await context.Genres.Where(d => d.ID == genreId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGenre_ExistingIdCorrectGenreWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int genreId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToUpdate = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                genreToUpdate.ID = idChanged;

                GenresController genresController = new GenresController(context);
                var result = (await genresController.PutGenre(genreId, genreToUpdate));

                Genre actualGenre = await context.Genres.Where(d => d.ID == genreId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutGenre_ExistingIdCorrectGenreWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int genreId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToUpdate = GetFakeList().Where(d => d.ID == genreId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                genreToUpdate.ID = idChanged;

                GenresController genresController = new GenresController(context);
                var result = (await genresController.PutGenre(genreId, genreToUpdate));

                Genre actualGenre = await context.Genres.Where(d => d.ID == genreId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGenre_CorrectGenreWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToCreate = new Genre()
            {
                Name = "NewDevelope"
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.PostGenre(genreToCreate)).Result;

                // Assert
                Assert.True(context.Genres.Contains(genreToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGenre_CorrectGenreWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToCreate = new Genre()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.PostGenre(genreToCreate)).Result;

                // Assert
                Assert.True(context.Genres.Contains(genreToCreate));
                Assert.True(genreToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGenre_CorrectGenreWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToCreate = new Genre()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.PostGenre(genreToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostGenre_CorrectGenreWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToCreate = new Genre()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.PostGenre(genreToCreate)).Result;

                // Assert
                Assert.True(genreToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostGenre_CorrectGenreWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Genre genreToCreate = new Genre()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var result = (await genresController.PostGenre(genreToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteGenre_ExistingId_TaskActionResultContainsDeletedGenre()
        {
            // Arrange
            const int genreIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Genre expectedGenre = GetFakeList().Where(d => d.ID == genreIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var actionResult = (await genresController.DeleteGenre(genreIdToDelete));
                var result = actionResult.Result;
                Genre actualGenre = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Genres.Find(genreIdToDelete) == null);
                Assert.True(AreEqual(expectedGenre, actualGenre));
            }
        }

        [Fact]
        public async Task DeleteGenre_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int genreIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Genre expectedGenre = GetFakeList().Where(d => d.ID == genreIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var actionResult = (await genresController.DeleteGenre(genreIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteGenre_ZeroId_NotFoundResult()
        {
            // Arrange
            const int genreIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Genre expectedGenre = GetFakeList().Where(d => d.ID == genreIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var actionResult = (await genresController.DeleteGenre(genreIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteGenre_NegativeId_NotFoundResult()
        {
            // Arrange
            const int genreIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Genre expectedGenre = GetFakeList().Where(d => d.ID == genreIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                GenresController genresController = new GenresController(context);
                var actionResult = (await genresController.DeleteGenre(genreIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Genre> GetFakeList()
        {
            var data = new List<Genre>
            {
                new Genre { Name = "BBB", ID = 2,  GameGenres = new List<GameGenre>() },
                new Genre { Name = "ZZZ", ID = 3,  GameGenres = new List<GameGenre>() },
                new Genre { Name = "AAA", ID = 4,  GameGenres = new List<GameGenre>() }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "GenresDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Genres.Add(new Genre { Name = "BBB", ID = 2, GameGenres = new List<GameGenre>() });
                context.Genres.Add(new Genre { Name = "ZZZ", ID = 3, GameGenres = new List<GameGenre>() });
                context.Genres.Add(new Genre { Name = "AAA", ID = 4, GameGenres = new List<GameGenre>() });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Genre> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "GenresDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Genre genre in values)
                    context.Genres.Add(genre);

                context.SaveChanges();
            }
        }

        private bool AreEqual(Genre expected, Genre actual)
        {
            bool areEqual = true;

            if ((expected == null && actual != null) || (expected != null && actual == null))
                areEqual = false;
            else if (expected.ID != actual.ID || expected.Name != actual.Name)
                areEqual = false;

            return areEqual;
        }
    }
}
