using awwcore_azure.Controllers;
using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace awwcore_azure.Tests.ControllersTests
{
    public class DevelopersControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetDevelopers_Void_TaskActionResultContainsIEnumerableOfDeveloper()
        {
            // Arrange
            List<Developer> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());        

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                IEnumerable<Developer> developers = (await developersController.GetDevelopers()).Value;

                // Assert
                Assert.Equal(expectedData.Count, developers.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], developers.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetDeveloper_ExistingId_TaskActionResultContainsDeveloper()
        {
            // Arrange
            const int developerId = 3;

            Developer expectedDeveloper = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                Developer developer = (await developersController.GetDeveloper(developerId)).Value;
                ActionResult result = (await developersController.GetDeveloper(developerId)).Result;

                // Assert              
                Assert.True(AreEqual(expectedDeveloper, developer));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetDeveloper_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int developerId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.GetDeveloper(developerId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetDeveloper_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int developerId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.GetDeveloper(developerId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetDeveloper_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int developerId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.GetDeveloper(developerId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithNameChanged_NoContentResult()
        {
            // Arrange
            const int developerId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedDeveloper = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.Name = "newName";
                expectedDeveloper.Name = "newName";

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedDeveloper, actualDeveloper));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithWebsiteChanged_NoContentResult()
        {
            // Arrange
            const int developerId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedDeveloper = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.Website = "newWebsite";
                expectedDeveloper.Website = "newWebsite";

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedDeveloper, actualDeveloper));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int developerId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.ID = idChanged;

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int developerId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.ID = idChanged;

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int developerId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.ID = idChanged;

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutDeveloper_ExistingIdCorrectDeveloperWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int developerId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToUpdate = GetFakeList().Where(d => d.ID == developerId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                developerToUpdate.ID = idChanged;

                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PutDeveloper(developerId, developerToUpdate));

                Developer actualDeveloper = await context.Developers.Where(d => d.ID == developerId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostDeveloper_CorrectDeveloperWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToCreate = new Developer()
            {
                Name = "NewDevelope",
                Website = "NewWebsite"
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PostDeveloper(developerToCreate)).Result;

                // Assert
                Assert.True(context.Developers.Contains(developerToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostDeveloper_CorrectDeveloperWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToCreate = new Developer()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PostDeveloper(developerToCreate)).Result;

                // Assert
                Assert.True(context.Developers.Contains(developerToCreate));
                Assert.True(developerToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostDeveloper_CorrectDeveloperWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToCreate = new Developer()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PostDeveloper(developerToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostDeveloper_CorrectDeveloperWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToCreate = new Developer()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var result = (await developersController.PostDeveloper(developerToCreate)).Result;

                // Assert
                Assert.True(developerToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostDeveloper_CorrectDeveloperWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Developer developerToCreate = new Developer()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                    DevelopersController developersController = new DevelopersController(context);
                    var result = (await developersController.PostDeveloper(developerToCreate)).Result;

                    // Assert
                    Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteDeveloper_ExistingId_TaskActionResultContainsDeletedDeveloper()
        {
            // Arrange
            const int developerIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Developer expectedDeveloper = GetFakeList().Where(d => d.ID == developerIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var actionResult = (await developersController.DeleteDeveloper(developerIdToDelete));
                var result = actionResult.Result;
                Developer actualDeveloper = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Developers.Find(developerIdToDelete) == null);
                Assert.True(AreEqual(expectedDeveloper, actualDeveloper));
            }
        }

        [Fact]
        public async Task DeleteDeveloper_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int developerIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Developer expectedDeveloper = GetFakeList().Where(d => d.ID == developerIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var actionResult = (await developersController.DeleteDeveloper(developerIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteDeveloper_ZeroId_NotFoundResult()
        {
            // Arrange
            const int developerIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Developer expectedDeveloper = GetFakeList().Where(d => d.ID == developerIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var actionResult = (await developersController.DeleteDeveloper(developerIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteDeveloper_NegativeId_NotFoundResult()
        {
            // Arrange
            const int developerIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Developer expectedDeveloper = GetFakeList().Where(d => d.ID == developerIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                DevelopersController developersController = new DevelopersController(context);
                var actionResult = (await developersController.DeleteDeveloper(developerIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Developer> GetFakeList()
        {
            var data = new List<Developer>
            {
                new Developer { Name = "BBB", ID = 2, Website = "DdD" },
                new Developer { Name = "ZZZ", ID = 3, Website = "OOo" },
                new Developer { Name = "AAA", ID = 4, Website = "-><" }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "DevelopersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Developers.Add(new Developer { Name = "BBB", ID = 2, Website = "DdD" });
                context.Developers.Add(new Developer { Name = "ZZZ", ID = 3, Website = "OOo" });
                context.Developers.Add(new Developer { Name = "AAA", ID = 4, Website = "-><" });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Developer> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "DevelopersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Developer developer in values)
                    context.Developers.Add(developer);

                context.SaveChanges();
            }
        }

        private bool AreEqual(Developer expected, Developer actual)
        {
            bool areEqual = true;

            if ((expected == null && actual != null) || (expected != null && actual == null))
                areEqual = false;
            else if (expected.ID != actual.ID || expected.Name != actual.Name || expected.Website != actual.Website)
                areEqual = false;

            return areEqual;
        }
    }
}