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
    public class PlatformsControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetPlatforms_Void_TaskActionResultContainsIEnumerableOfPlatform()
        {
            // Arrange
            List<Platform> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                IEnumerable<Platform> platforms = (await platformsController.GetPlatforms()).Value;

                // Assert
                Assert.Equal(expectedData.Count, platforms.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], platforms.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetPlatform_ExistingId_TaskActionResultContainsPlatform()
        {
            // Arrange
            const int platformId = 3;

            Platform expectedPlatform = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                Platform platform = (await platformsController.GetPlatform(platformId)).Value;
                ActionResult result = (await platformsController.GetPlatform(platformId)).Result;

                // Assert              
                Assert.True(AreEqual(expectedPlatform, platform));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPlatform_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int platformId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.GetPlatform(platformId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPlatform_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int platformId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.GetPlatform(platformId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPlatform_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int platformId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.GetPlatform(platformId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutPlatform_ExistingIdCorrectPlatformWithNameChanged_NoContentResult()
        {
            // Arrange
            const int platformId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToUpdate = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedPlatform = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                platformToUpdate.Name = "newName";
                expectedPlatform.Name = "newName";

                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PutPlatform(platformId, platformToUpdate));

                Platform actualPlatform = await context.Platforms.Where(d => d.ID == platformId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedPlatform, actualPlatform));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutPlatform_ExistingIdCorrectPlatformWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int platformId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToUpdate = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                platformToUpdate.ID = idChanged;

                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PutPlatform(platformId, platformToUpdate));

                Platform actualPlatform = await context.Platforms.Where(d => d.ID == platformId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPlatform_ExistingIdCorrectPlatformWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int platformId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToUpdate = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                platformToUpdate.ID = idChanged;

                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PutPlatform(platformId, platformToUpdate));

                Platform actualPlatform = await context.Platforms.Where(d => d.ID == platformId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPlatform_ExistingIdCorrectPlatformWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int platformId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToUpdate = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                platformToUpdate.ID = idChanged;

                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PutPlatform(platformId, platformToUpdate));

                Platform actualPlatform = await context.Platforms.Where(d => d.ID == platformId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPlatform_ExistingIdCorrectPlatformWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int platformId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToUpdate = GetFakeList().Where(d => d.ID == platformId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                platformToUpdate.ID = idChanged;

                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PutPlatform(platformId, platformToUpdate));

                Platform actualPlatform = await context.Platforms.Where(d => d.ID == platformId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostPlatform_CorrectPlatformWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToCreate = new Platform()
            {
                Name = "NewDevelope"
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PostPlatform(platformToCreate)).Result;

                // Assert
                Assert.True(context.Platforms.Contains(platformToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPlatform_CorrectPlatformWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToCreate = new Platform()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PostPlatform(platformToCreate)).Result;

                // Assert
                Assert.True(context.Platforms.Contains(platformToCreate));
                Assert.True(platformToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPlatform_CorrectPlatformWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToCreate = new Platform()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PostPlatform(platformToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostPlatform_CorrectPlatformWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToCreate = new Platform()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PostPlatform(platformToCreate)).Result;

                // Assert
                Assert.True(platformToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPlatform_CorrectPlatformWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Platform platformToCreate = new Platform()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var result = (await platformsController.PostPlatform(platformToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeletePlatform_ExistingId_TaskActionResultContainsDeletedPlatform()
        {
            // Arrange
            const int platformIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Platform expectedPlatform = GetFakeList().Where(d => d.ID == platformIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var actionResult = (await platformsController.DeletePlatform(platformIdToDelete));
                var result = actionResult.Result;
                Platform actualPlatform = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Platforms.Find(platformIdToDelete) == null);
                Assert.True(AreEqual(expectedPlatform, actualPlatform));
            }
        }

        [Fact]
        public async Task DeletePlatform_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int platformIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Platform expectedPlatform = GetFakeList().Where(d => d.ID == platformIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var actionResult = (await platformsController.DeletePlatform(platformIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeletePlatform_ZeroId_NotFoundResult()
        {
            // Arrange
            const int platformIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Platform expectedPlatform = GetFakeList().Where(d => d.ID == platformIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var actionResult = (await platformsController.DeletePlatform(platformIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeletePlatform_NegativeId_NotFoundResult()
        {
            // Arrange
            const int platformIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Platform expectedPlatform = GetFakeList().Where(d => d.ID == platformIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PlatformsController platformsController = new PlatformsController(context);
                var actionResult = (await platformsController.DeletePlatform(platformIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Platform> GetFakeList()
        {
            var data = new List<Platform>
            {
                new Platform { Name = "BBB", ID = 2 },
                new Platform { Name = "ZZZ", ID = 3 },
                new Platform { Name = "AAA", ID = 4 }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "PlatformsDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Platforms.Add(new Platform { Name = "BBB", ID = 2 });
                context.Platforms.Add(new Platform { Name = "ZZZ", ID = 3 });
                context.Platforms.Add(new Platform { Name = "AAA", ID = 4 });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Platform> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "PlatformsDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Platform platform in values)
                    context.Platforms.Add(platform);

                context.SaveChanges();
            }
        }

        private bool AreEqual(Platform expected, Platform actual)
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
