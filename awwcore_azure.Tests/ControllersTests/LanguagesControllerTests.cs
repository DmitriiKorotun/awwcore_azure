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
    public class LanguagesControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetLanguages_Void_TaskActionResultContainsIEnumerableOfLanguage()
        {
            // Arrange
            List<Language> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                IEnumerable<Language> languages = (await languagesController.GetLanguages()).Value;

                // Assert
                Assert.Equal(expectedData.Count, languages.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], languages.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetLanguage_ExistingId_TaskActionResultContainsLanguage()
        {
            // Arrange
            const int languageId = 3;

            Language expectedLanguage = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var actionResult = (await languagesController.GetLanguage(languageId));
                Language language = actionResult.Value;
                ActionResult result = actionResult.Result;

                // Assert              
                Assert.True(AreEqual(expectedLanguage, language));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetLanguage_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int languageId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.GetLanguage(languageId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetLanguage_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int languageId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.GetLanguage(languageId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetLanguage_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int languageId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.GetLanguage(languageId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutLanguage_ExistingIdCorrectLanguageWithNameChanged_NoContentResult()
        {
            // Arrange
            const int languageId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToUpdate = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedLanguage = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                languageToUpdate.Name = "newName";
                expectedLanguage.Name = "newName";

                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PutLanguage(languageId, languageToUpdate));

                Language actualLanguage = await context.Languages.Where(d => d.ID == languageId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedLanguage, actualLanguage));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutLanguage_ExistingIdCorrectLanguageWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int languageId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToUpdate = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                languageToUpdate.ID = idChanged;

                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PutLanguage(languageId, languageToUpdate));

                Language actualLanguage = await context.Languages.Where(d => d.ID == languageId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutLanguage_ExistingIdCorrectLanguageWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int languageId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToUpdate = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                languageToUpdate.ID = idChanged;

                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PutLanguage(languageId, languageToUpdate));

                Language actualLanguage = await context.Languages.Where(d => d.ID == languageId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutLanguage_ExistingIdCorrectLanguageWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int languageId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToUpdate = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                languageToUpdate.ID = idChanged;

                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PutLanguage(languageId, languageToUpdate));

                Language actualLanguage = await context.Languages.Where(d => d.ID == languageId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutLanguage_ExistingIdCorrectLanguageWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int languageId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToUpdate = GetFakeList().Where(d => d.ID == languageId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                languageToUpdate.ID = idChanged;

                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PutLanguage(languageId, languageToUpdate));

                Language actualLanguage = await context.Languages.Where(d => d.ID == languageId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostLanguage_CorrectLanguageWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToCreate = new Language()
            {
                Name = "NewDevelope"
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PostLanguage(languageToCreate)).Result;

                // Assert
                Assert.True(context.Languages.Contains(languageToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostLanguage_CorrectLanguageWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToCreate = new Language()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PostLanguage(languageToCreate)).Result;

                // Assert
                Assert.True(context.Languages.Contains(languageToCreate));
                Assert.True(languageToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostLanguage_CorrectLanguageWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToCreate = new Language()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PostLanguage(languageToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostLanguage_CorrectLanguageWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToCreate = new Language()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PostLanguage(languageToCreate)).Result;

                // Assert
                Assert.True(languageToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostLanguage_CorrectLanguageWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Language languageToCreate = new Language()
            {
                Name = "NewDevelope",
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var result = (await languagesController.PostLanguage(languageToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteLanguage_ExistingId_TaskActionResultContainsDeletedLanguage()
        {
            // Arrange
            const int languageIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Language expectedLanguage = GetFakeList().Where(d => d.ID == languageIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var actionResult = (await languagesController.DeleteLanguage(languageIdToDelete));
                var result = actionResult.Result;
                Language actualLanguage = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Languages.Find(languageIdToDelete) == null);
                Assert.True(AreEqual(expectedLanguage, actualLanguage));
            }
        }

        [Fact]
        public async Task DeleteLanguage_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int languageIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Language expectedLanguage = GetFakeList().Where(d => d.ID == languageIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var actionResult = (await languagesController.DeleteLanguage(languageIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteLanguage_ZeroId_NotFoundResult()
        {
            // Arrange
            const int languageIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Language expectedLanguage = GetFakeList().Where(d => d.ID == languageIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var actionResult = (await languagesController.DeleteLanguage(languageIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteLanguage_NegativeId_NotFoundResult()
        {
            // Arrange
            const int languageIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Language expectedLanguage = GetFakeList().Where(d => d.ID == languageIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                LanguagesController languagesController = new LanguagesController(context);
                var actionResult = (await languagesController.DeleteLanguage(languageIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Language> GetFakeList()
        {
            var data = new List<Language>
            {
                new Language { Name = "BBB", ID = 2 },
                new Language { Name = "ZZZ", ID = 3 },
                new Language { Name = "AAA", ID = 4 }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "LanguagesDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Languages.Add(new Language { Name = "BBB", ID = 2 });
                context.Languages.Add(new Language { Name = "ZZZ", ID = 3 });
                context.Languages.Add(new Language { Name = "AAA", ID = 4 });
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Language> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "LanguagesDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Language language in values)
                    context.Languages.Add(language);

                context.SaveChanges();
            }
        }

        private bool AreEqual(Language expected, Language actual)
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
