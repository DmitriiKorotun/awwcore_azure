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
    public class PublishersControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetPublishers_Void_TaskActionResultContainsIEnumerableOfPublisher()
        {
            // Arrange
            List<Publisher> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                IEnumerable<Publisher> publishers = (await publishersController.GetPublishers()).Value;

                // Assert
                Assert.Equal(expectedData.Count, publishers.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], publishers.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetPublisher_ExistingId_TaskActionResultContainsPublisher()
        {
            // Arrange
            const int publisherId = 3;

            Publisher expectedPublisher = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var actionResult = (await publishersController.GetPublisher(publisherId));
                Publisher publisher = actionResult.Value;
                ActionResult result = actionResult.Result;

                // Assert              
                Assert.True(AreEqual(expectedPublisher, publisher));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPublisher_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int publisherId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.GetPublisher(publisherId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPublisher_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int publisherId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.GetPublisher(publisherId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetPublisher_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int publisherId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.GetPublisher(publisherId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithNameChanged_NoContentResult()
        {
            // Arrange
            const int publisherId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedPublisher = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.Name = "newName";
                expectedPublisher.Name = "newName";

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedPublisher, actualPublisher));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithWebsiteChanged_NoContentResult()
        {
            // Arrange
            const int publisherId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedPublisher = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.Website = "newWebsite";
                expectedPublisher.Website = "newWebsite";

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedPublisher, actualPublisher));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int publisherId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.ID = idChanged;

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int publisherId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.ID = idChanged;

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int publisherId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.ID = idChanged;

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutPublisher_ExistingIdCorrectPublisherWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int publisherId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToUpdate = GetFakeList().Where(d => d.ID == publisherId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                publisherToUpdate.ID = idChanged;

                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PutPublisher(publisherId, publisherToUpdate));

                Publisher actualPublisher = await context.Publishers.Where(d => d.ID == publisherId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostPublisher_CorrectPublisherWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToCreate = new Publisher()
            {
                Name = "NewDevelope",
                Website = "NewWebsite"
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PostPublisher(publisherToCreate)).Result;

                // Assert
                Assert.True(context.Publishers.Contains(publisherToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPublisher_CorrectPublisherWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToCreate = new Publisher()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PostPublisher(publisherToCreate)).Result;

                // Assert
                Assert.True(context.Publishers.Contains(publisherToCreate));
                Assert.True(publisherToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPublisher_CorrectPublisherWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToCreate = new Publisher()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PostPublisher(publisherToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostPublisher_CorrectPublisherWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToCreate = new Publisher()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PostPublisher(publisherToCreate)).Result;

                // Assert
                Assert.True(publisherToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostPublisher_CorrectPublisherWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Publisher publisherToCreate = new Publisher()
            {
                Name = "NewDevelope",
                Website = "NewWebsite",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var result = (await publishersController.PostPublisher(publisherToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeletePublisher_ExistingId_TaskActionResultContainsDeletedPublisher()
        {
            // Arrange
            const int publisherIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Publisher expectedPublisher = GetFakeList().Where(d => d.ID == publisherIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var actionResult = (await publishersController.DeletePublisher(publisherIdToDelete));
                var result = actionResult.Result;
                Publisher actualPublisher = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Publishers.Find(publisherIdToDelete) == null);
                Assert.True(AreEqual(expectedPublisher, actualPublisher));
            }
        }

        [Fact]
        public async Task DeletePublisher_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int publisherIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Publisher expectedPublisher = GetFakeList().Where(d => d.ID == publisherIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var actionResult = (await publishersController.DeletePublisher(publisherIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeletePublisher_ZeroId_NotFoundResult()
        {
            // Arrange
            const int publisherIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Publisher expectedPublisher = GetFakeList().Where(d => d.ID == publisherIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var actionResult = (await publishersController.DeletePublisher(publisherIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeletePublisher_NegativeId_NotFoundResult()
        {
            // Arrange
            const int publisherIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Publisher expectedPublisher = GetFakeList().Where(d => d.ID == publisherIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                PublishersController publishersController = new PublishersController(context);
                var actionResult = (await publishersController.DeletePublisher(publisherIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Publisher> GetFakeList()
        {
            var data = new List<Publisher>
            {
                new Publisher { Name = "BBB", ID = 2, Website = "DdD" },
                new Publisher { Name = "ZZZ", ID = 3, Website = "OOo" },
                new Publisher { Name = "AAA", ID = 4, Website = "-><" }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "PublishersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Publishers.Add(new Publisher { Name = "BBB", ID = 2, Website = "DdD" });
                context.Publishers.Add(new Publisher { Name = "ZZZ", ID = 3, Website = "OOo" });
                context.Publishers.Add(new Publisher { Name = "AAA", ID = 4, Website = "-><" });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Publisher> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "PublishersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Publisher publisher in values)
                    context.Publishers.Add(publisher);

                context.SaveChanges();
            }
        }

        private bool AreEqual(Publisher expected, Publisher actual)
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