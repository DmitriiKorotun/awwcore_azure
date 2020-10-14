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
    public class UsersControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetUsers_Void_TaskActionResultContainsIEnumerableOfUser()
        {
            // Arrange
            List<User> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                IEnumerable<User> users = (await usersController.GetUsers()).Value;

                // Assert
                Assert.Equal(expectedData.Count, users.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], users.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetUser_ExistingId_TaskActionResultContainsUser()
        {
            // Arrange
            const int userId = 3;

            User expectedUser = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var actionResult = (await usersController.GetUser(userId));
                User user = actionResult.Value;
                ActionResult result = actionResult.Result;

                // Assert              
                Assert.True(AreEqual(expectedUser, user));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetUser_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int userId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.GetUser(userId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetUser_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int userId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.GetUser(userId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetUser_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int userId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.GetUser(userId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithNameChanged_NoContentResult()
        {
            // Arrange
            const int userId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedUser = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.Name = "newName";
                expectedUser.Name = "newName";

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedUser, actualUser));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithWebsiteChanged_NoContentResult()
        {
            // Arrange
            const int userId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedUser = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.RegistrationDate = DateTime.Parse("15-10-2020");
                expectedUser.RegistrationDate = DateTime.Parse("15-10-2020");

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedUser, actualUser));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int userId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.ID = idChanged;

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int userId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.ID = idChanged;

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int userId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.ID = idChanged;

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutUser_ExistingIdCorrectUserWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int userId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToUpdate = GetFakeList().Where(d => d.ID == userId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                userToUpdate.ID = idChanged;

                UsersController usersController = new UsersController(context);
                var result = (await usersController.PutUser(userId, userToUpdate));

                User actualUser = await context.Users.Where(d => d.ID == userId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostUser_CorrectUserWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToCreate = new User()
            {
                Name = "NewDevelope",
                RegistrationDate = DateTime.Parse("15-10-2020")
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.PostUser(userToCreate)).Result;

                // Assert
                Assert.True(context.Users.Contains(userToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostUser_CorrectUserWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToCreate = new User()
            {
                Name = "NewDevelope",
                RegistrationDate = DateTime.Parse("15-10-2020"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.PostUser(userToCreate)).Result;

                // Assert
                Assert.True(context.Users.Contains(userToCreate));
                Assert.True(userToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostUser_CorrectUserWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToCreate = new User()
            {
                Name = "NewDevelope",
                RegistrationDate = DateTime.Parse("15-10-2020"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.PostUser(userToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostUser_CorrectUserWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToCreate = new User()
            {
                Name = "NewDevelope",
                RegistrationDate = DateTime.Parse("15-10-2020"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.PostUser(userToCreate)).Result;

                // Assert
                Assert.True(userToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostUser_CorrectUserWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            User userToCreate = new User()
            {
                Name = "NewDevelope",
                RegistrationDate = DateTime.Parse("15-10-2020"),
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var result = (await usersController.PostUser(userToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteUser_ExistingId_TaskActionResultContainsDeletedUser()
        {
            // Arrange
            const int userIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            User expectedUser = GetFakeList().Where(d => d.ID == userIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var actionResult = (await usersController.DeleteUser(userIdToDelete));
                var result = actionResult.Result;
                User actualUser = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Users.Find(userIdToDelete) == null);
                Assert.True(AreEqual(expectedUser, actualUser));
            }
        }

        [Fact]
        public async Task DeleteUser_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int userIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            User expectedUser = GetFakeList().Where(d => d.ID == userIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var actionResult = (await usersController.DeleteUser(userIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteUser_ZeroId_NotFoundResult()
        {
            // Arrange
            const int userIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            User expectedUser = GetFakeList().Where(d => d.ID == userIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var actionResult = (await usersController.DeleteUser(userIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteUser_NegativeId_NotFoundResult()
        {
            // Arrange
            const int userIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            User expectedUser = GetFakeList().Where(d => d.ID == userIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                UsersController usersController = new UsersController(context);
                var actionResult = (await usersController.DeleteUser(userIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<User> GetFakeList()
        {
            var data = new List<User>
            {
                new User { Name = "BBB", ID = 2, RegistrationDate = DateTime.Parse("04-03-2017") },
                new User { Name = "ZZZ", ID = 3, RegistrationDate = DateTime.Parse("28-02-2015") },
                new User { Name = "AAA", ID = 4, RegistrationDate = DateTime.Parse("13-07-2001") }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "UsersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Users.Add(new User { Name = "BBB", ID = 2, RegistrationDate = DateTime.Parse("04-03-2017") });
                context.Users.Add(new User { Name = "ZZZ", ID = 3, RegistrationDate = DateTime.Parse("28-02-2015") });
                context.Users.Add(new User { Name = "AAA", ID = 4, RegistrationDate = DateTime.Parse("13-07-2001") });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<User> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "UsersDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (User user in values)
                    context.Users.Add(user);

                context.SaveChanges();
            }
        }

        private bool AreEqual(User expected, User actual)
        {
            bool areEqual = true;

            if ((expected == null && actual != null) || (expected != null && actual == null))
                areEqual = false;
            else if (expected.ID != actual.ID || expected.Name != actual.Name || !expected.RegistrationDate.Equals(actual.RegistrationDate))
                areEqual = false;

            return areEqual;
        }
    }
}
