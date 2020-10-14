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
    public class ReviewsControllerTests
    {
        // Index to add to inmemory database name in InitializeInmemoryDatabase method to isolate entities in their own database for each test
        private static int InMemoryDbIndex { get; set; }

        [Fact]
        public async Task GetReviews_Void_TaskActionResultContainsIEnumerableOfReview()
        {
            // Arrange
            List<Review> expectedData = GetFakeList();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                IEnumerable<Review> reviews = (await reviewsController.GetReviews()).Value;

                // Assert
                Assert.Equal(expectedData.Count, reviews.Count());

                for (int i = 0; i < expectedData.Count; ++i)
                    Assert.True(AreEqual(expectedData[i], reviews.ElementAt(i)));
            }
        }

        [Fact]
        public async Task GetReview_ExistingId_TaskActionResultContainsReview()
        {
            // Arrange
            const int reviewId = 3;

            Review expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var actionResult = await reviewsController.GetReview(reviewId);
                Review review = actionResult.Value;
                ActionResult result = actionResult.Result;

                // Assert              
                Assert.True(AreEqual(expectedReview, review));
                Assert.IsNotType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetReview_NonexistentId_NotFoundResult()
        {
            // Arrange
            const int reviewId = 5;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.GetReview(reviewId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetReview_NonexistentZeroId_NotFoundResult()
        {
            // Arrange
            const int reviewId = 0;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.GetReview(reviewId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task GetReview_NonexistentNegativeId_NotFoundResult()
        {
            // Arrange
            const int reviewId = -1;

            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.GetReview(reviewId)).Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithNameChanged_NoContentResult()
        {
            // Arrange
            const int reviewId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();


            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.Name = "newName";
                expectedReview.Name = "newName";

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithTextChanged_NoContentResult()
        {
            // Arrange
            const int reviewId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.Text = "newText";
                expectedReview.Text = "newText";

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithAnnouncementDateChanged_NoContentResult()
        {
            // Arrange
            const int reviewId = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.IsRecommend = !reviewToUpdate.IsRecommend;
                expectedReview.IsRecommend = reviewToUpdate.IsRecommend;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithGameIdChangedToExisting_NoContentResultGameIdChanged()
        {
            // Arrange
            const int reviewId = 3, gameId = 7;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.GameId = gameId;
                expectedReview.GameId = gameId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithGameIdChangedToNonexisting_NoContentResultGameIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, gameId = 20;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.GameId = gameId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithGameIdChangedToZero_NoContentResultGameIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, gameId = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.GameId = gameId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithGameIdChangedToNegative_NoContentResultGameIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, gameId = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.GameId = gameId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }


        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithUserIdChangedToExisting_NoContentResultUserIdChanged()
        {
            // Arrange
            const int reviewId = 3, userId = 7;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.UserId = userId;
                expectedReview.UserId = userId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithUserIdChangedToNonexisting_NoContentResultUserIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, userId = 20;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.UserId = userId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithUserIdChangedToZero_NoContentResultUserIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, userId = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.UserId = userId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithUserIdChangedToNegative_NoContentResultUserIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, userId = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.UserId = userId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithLanguageIdChangedToExisting_NoContentResultLanguageIdChanged()
        {
            // Arrange
            const int reviewId = 3, languageId = 7;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.LanguageId = languageId;
                expectedReview.LanguageId = languageId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.True(AreEqual(expectedReview, actualReview));
                Assert.IsType<NoContentResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithLanguageIdChangedToNonexisting_NoContentResultLanguageIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, languageId = 20;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.LanguageId = languageId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithLanguageIdChangedToZero_NoContentResultLanguageIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, languageId = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.LanguageId = languageId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithLanguageIdChangedToNegative_NoContentResultLanguageIdDoesntChanged()
        {
            // Arrange
            const int reviewId = 3, languageId = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault(),
                // Should use ICopyable interface here
                expectedReview = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.LanguageId = languageId;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithIdChangedExisting_BadRequestResult()
        {
            // Arrange
            const int reviewId = 3, idChanged = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.ID = idChanged;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithIdChangedNonexistent_BadRequestResult()
        {
            // Arrange
            const int reviewId = 3, idChanged = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.ID = idChanged;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithIdChangedZero_BadRequestResult()
        {
            // Arrange
            const int reviewId = 3, idChanged = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.ID = idChanged;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PutReview_ExistingIdCorrectReviewWithIdChangedNegative_BadRequestResult()
        {
            // Arrange
            const int reviewId = 3, idChanged = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToUpdate = GetFakeList().Where(d => d.ID == reviewId).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                reviewToUpdate.ID = idChanged;

                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PutReview(reviewId, reviewToUpdate));

                Review actualReview = await context.Reviews.Where(d => d.ID == reviewId).FirstOrDefaultAsync();

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesAndIdNotSet_CreatedAtActionResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.True(context.Reviews.Contains(reviewToCreate));
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesAndNonexistingIdSetted_CreatedAtActionResultWithSettedId()
        {
            // Arrange
            const int idToSet = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.True(context.Reviews.Contains(reviewToCreate));
                Assert.True(reviewToCreate.ID == idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesAndExistingIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = 4;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesAndZeroIdSetted_CreatedAtActionResultWithGeneratedId()
        {
            // Arrange
            const int idToSet = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.True(reviewToCreate.ID != idToSet);
                Assert.IsType<CreatedAtActionResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesAndNegativeIdSetted_BadRequestResult()
        {
            // Arrange
            const int idToSet = -1;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
                ID = idToSet
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNonexistingGameIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 20,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptZeroGameIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 0,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNegativeGameIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = -1,
                UserId = 2,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNonexistingDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = 20,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptZeroDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = 0,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNegativeDeveloperIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = -1,
                LanguageId = 3,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNonexistingLanguageIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = 1,
                LanguageId = 30,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptZeroLanguageIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = 1,
                LanguageId = 0,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task PostReview_CorrectReviewWithCorrectValuesExceptNegativeLanguageIdAndIdNotSet_BadRequestResult()
        {
            // Arrange
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());

            Review reviewToCreate = new Review()
            {
                Name = "NewReview",
                Text = "NewText",
                GameId = 2,
                UserId = 1,
                LanguageId = -1,
                IsRecommend = true,
            };

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var result = (await reviewsController.PostReview(reviewToCreate)).Result;

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }

        [Fact]
        public async Task DeleteReview_ExistingId_TaskActionResultContainsDeletedReview()
        {
            // Arrange
            const int reviewIdToDelete = 3;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Review expectedReview = GetFakeList().Where(d => d.ID == reviewIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var actionResult = (await reviewsController.DeleteReview(reviewIdToDelete));
                var result = actionResult.Result;
                Review actualReview = actionResult.Value;

                // Assert
                Assert.IsNotType<NotFoundResult>(result);
                Assert.True(context.Reviews.Find(reviewIdToDelete) == null);
                Assert.True(AreEqual(expectedReview, actualReview));
            }
        }

        [Fact]
        public async Task DeleteReview_NonexistingId_NotFoundResult()
        {
            // Arrange
            const int reviewIdToDelete = 10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Review expectedReview = GetFakeList().Where(d => d.ID == reviewIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var actionResult = (await reviewsController.DeleteReview(reviewIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }


        [Fact]
        public async Task DeleteReview_ZeroId_NotFoundResult()
        {
            // Arrange
            const int reviewIdToDelete = 0;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Review expectedReview = GetFakeList().Where(d => d.ID == reviewIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var actionResult = (await reviewsController.DeleteReview(reviewIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public async Task DeleteReview_NegativeId_NotFoundResult()
        {
            // Arrange
            const int reviewIdToDelete = -10;
            InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, GetFakeList());
            Review expectedReview = GetFakeList().Where(d => d.ID == reviewIdToDelete).FirstOrDefault();

            // Act
            using (var context = new GameReviewsContext(options))
            {
                ReviewsController reviewsController = new ReviewsController(context);
                var actionResult = (await reviewsController.DeleteReview(reviewIdToDelete));
                var result = actionResult.Result;

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // There are no reasons to test model annotations cause in-memory database doesn't do any app-level validation  

        private List<Review> GetFakeList()
        {
            var data = new List<Review>
            {
                new Review { Name = "BBB", ID = 2, Text = "DdD",
                    IsRecommend = true, GameId = 1,
                    UserId = 2, LanguageId = 3 },

                new Review { Name = "ZZZ", ID = 3, Text = "OOo",
                    IsRecommend = false, GameId = 2,
                    UserId = 1, LanguageId = 3 },

                new Review { Name = "AAA", ID = 4, Text = "-><",
                    IsRecommend = false, GameId = 3,
                    UserId = 1, LanguageId = 4 }
            };

            return data;
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "ReviewsDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                context.Reviews.Add(new Review
                {
                    Name = "BBB",
                    ID = 2,
                    Text = "DdD",
                    IsRecommend = true,
                    GameId = 1,
                    UserId = 2,
                    LanguageId = 3
                });
                context.Reviews.Add(new Review
                {
                    Name = "ZZZ",
                    ID = 3,
                    Text = "OOo",
                    IsRecommend = false,
                    GameId = 2,
                    UserId = 1,
                    LanguageId = 3
                });
                context.Reviews.Add(new Review
                {
                    Name = "AAA",
                    ID = 4,
                    Text = "-><",
                    IsRecommend = false,
                    GameId = 3,
                    UserId = 1,
                    LanguageId = 4
                });

                for(int i = 0; i < 15; ++i)
                {
                    context.Users.Add(new User() { Name = i.ToString(), RegistrationDate = DateTime.Now.AddDays(i) });
                    context.Languages.Add(new Language() { Name = i.ToString() });

                    context.Developers.Add(new Developer() { Name = i.ToString(), Website = "Website" + i });
                    context.Publishers.Add(new Publisher() { Name = i.ToString(), Website = "Website" + i });
                }

                for (int i = 0; i < 10; ++i)
                {
                    context.Games.Add(new Game
                    {
                        Name = "BBB" + i,
                        ID = 1 + i,
                        Description = "DdD" + i,
                        AnnouncementDate = DateTime.Parse("13-09-2019").AddDays(i),
                        ReleaseDate = DateTime.Parse("13-09-2019").AddDays(i),
                        DeveloperId = 2 + i,
                        PublisherId = 3 + i,
                        GameGenres = new List<GameGenre>(),
                        GamePlatforms = new List<GamePlatform>()
                    });
                }
                context.SaveChanges();
            }
        }

        private void InitializeInmemoryDatabase(out DbContextOptions<GameReviewsContext> options, IEnumerable<Review> values)
        {
            options = new DbContextOptionsBuilder<GameReviewsContext>()
                .UseInMemoryDatabase(databaseName: "ReviewsDatabase" + InMemoryDbIndex++)
                .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new GameReviewsContext(options))
            {
                foreach (Review review in values)
                    context.Reviews.Add(review);

                for (int i = 0; i < 15; ++i)
                {
                    context.Users.Add(new User() { Name = i.ToString(), RegistrationDate = DateTime.Now.AddDays(i) });
                    context.Languages.Add(new Language() { Name = i.ToString() });

                    context.Developers.Add(new Developer() { Name = i.ToString(), Website = "Website" + i });
                    context.Publishers.Add(new Publisher() { Name = i.ToString(), Website = "Website" + i });
                }

                for (int i = 0; i < 10; ++i)
                {
                    context.Games.Add(new Game
                    {
                        Name = "BBB" + i,
                        ID = 1 + i,
                        Description = "DdD" + i,
                        AnnouncementDate = DateTime.Parse("13-09-2019").AddDays(i),
                        ReleaseDate = DateTime.Parse("13-09-2019").AddDays(i),
                        DeveloperId = 2 + i,
                        PublisherId = 3 + i,
                        GameGenres = new List<GameGenre>(),
                        GamePlatforms = new List<GamePlatform>()
                    });
                }

                context.SaveChanges();
            }
        }

        private bool AreEqual(Review expected, Review actual)
        {
            bool areEqual = true;

            if ((expected == null && actual != null) || (expected != null && actual == null))
                areEqual = false;
            else if (expected.ID != actual.ID || expected.Name != actual.Name || expected.Text != actual.Text
                || expected.IsRecommend != actual.IsRecommend || expected.GameId != actual.GameId
                || expected.UserId != actual.UserId || expected.LanguageId != actual.LanguageId)
                areEqual = false;

            return areEqual;
        }
    }
}
