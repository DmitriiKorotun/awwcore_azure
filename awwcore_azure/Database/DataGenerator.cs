using awwcore_azure.Controllers;
using awwcore_azure.Database.Entities;
using awwcore_azure.Database.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database
{
    public class DataGenerator
    {
        private GameReviewsContext Context { get; set; }

        public DataGenerator(GameReviewsContext context)
        {
            Context = context;
        }

        public void GenerateDevelopers()
        {
            string[] developersNames = {
                "bethesda", "microsoft", "obisidan", "bioware", "activision", "larian", "paradox",
                "ubisoft", "blizzard", "valve", "bungie", "sony"
            };

            string[] developersWebsites = {
                "bethesda-site", "microsoft-site", "obisidan-site", "bioware-site", "activision-site",
                "larian-site", "paradox-site", "ubisoft-site", "blizzard-site", "valve-site"
            };

            for(int i = 0; i < developersNames.Length; ++i)
            {
                Developer developer = new Developer
                {
                    Name = developersNames[i]
                };

                if (i < developersWebsites.Length)
                    developer.Website = developersWebsites[i];

                Context.Developers.Add(developer);
            }

            Context.SaveChanges();
        }

        public void GenerateGameGenres()
        {
            Random rand = new Random();

            List<Game> games = Context.Games.ToList();
            List<Genre> genres = Context.Genres.ToList();

            foreach (Game game in games)
            {
                for (int i = 0; i < rand.Next(1, 4); ++i)
                {
                    if (i < genres.Count)
                    {
                        GameGenre gameGenre = new GameGenre
                        {
                            GameId = game.ID,
                            GenreId = genres[i].ID,
                        };

                        Context.GameGenres.Add(gameGenre);
                    }
                }
            }

            Context.SaveChanges();
        }

        public void GenerateGamePlatforms()
        {
            Random rand = new Random();

            List<Game> games = Context.Games.ToList();
            List<Platform> platforms = Context.Platforms.ToList();

            foreach (Game game in games)
            {
                for (int i = 0; i < rand.Next(1, 4); ++i)
                {
                    if (i < platforms.Count)
                    {
                        GamePlatform gamePlatform = new GamePlatform
                        {
                            GameId = game.ID,
                            PlatformId = platforms[i].ID,
                        };

                        Context.GamePlatforms.Add(gamePlatform);
                    }
                }
            }

            Context.SaveChanges();
        }

        public void GenerateGames()
        {
            Random rand = new Random();

            List<Publisher> publishers = Context.Publishers.ToList();
            List<Developer> developers = Context.Developers.ToList();

            string[] names = {
                "dishonored", "divinity", "ck", "europa", "stellaris", "hoi", "imperator",
                "baldurs gate", "mass effect", "darkest dungeon", "they are billions",
                "prey", "poe", "skyrim", "fallout"
            };

            string[] descriptionPartOne = {
                "awfull", "excellent", "brilliant", "boring", "the best", "the worst",
                "great",
                "unbeliaveble", "mindblowing", "unexpected", "creative",
                "usual", "unusual", "rare"
            };

            string[] descriptionPartTwo = {
                "experince", "plan", "emotions", "expectations", "idea", "game", "masterpiece",
                "invention", "drama", "comedy"
            };

            string[] descriptionPartThree = {
                "zombie", "friends", "foes", "ghosts", "citizens", "monsters", "animals",
                "rats", "people", "aliens", "creatures",
                "vampires", "murlocs", "elves", "things"
            };

            string[] descriptionPartFour = {
                "killing", "friendship", "history", "help", "life", "saving", "rescue",
                "search", "quests"
            };

            for (int i = 0; i < rand.Next(100); ++i)
            {
                Game game = new Game
                {
                    DeveloperId = developers[rand.Next(developers.Count)].ID,
                    PublisherId = publishers[rand.Next(publishers.Count)].ID,
                    Name = names[rand.Next(names.Length)],
                    Description = descriptionPartOne[rand.Next(descriptionPartOne.Length)]
                    + " " + descriptionPartTwo[rand.Next(descriptionPartTwo.Length)] + " of "
                    + descriptionPartThree[rand.Next(descriptionPartThree.Length)] + " "
                    + descriptionPartFour[rand.Next(descriptionPartFour.Length)],
                    AnnouncementDate = DateTime.Now.AddDays(-rand.Next(3000))
                };
                game.ReleaseDate = game.AnnouncementDate.AddDays(rand.Next(4000));

                Context.Games.Add(game);
            }

            Context.SaveChanges();
        }

        public void GenerateGenres()
        {
            string[] genres = {
                "shooter", "rpg", "fighting", "racing", "sport", "simulator", "sandbox",
                "action", "mmo", "moba", "card", "turn-based strategy", "realtime strategy",
                "puzzle", "horror"
            };

            foreach (string genreName in genres)
            {
                Genre genre = new Genre
                {
                    Name = genreName
                };

                Context.Genres.Add(genre);
            }

            Context.SaveChanges();
        }

        public void GenerateLanguages()
        {
            string[] languages = {
                "hindi", "bengali", "portuguese", "japanese", "punjabi", "marathi", "turkish",
                "korean", "french", "german", "polish", "ukrainian", "dutch",
                "greek", "czech"
            };

            foreach (string languageName in languages)
            {
                Language language = new Language
                {
                    Name = languageName
                };

                Context.Languages.Add(language);
            }

            Context.SaveChanges();
        }

        public void GeneratePlatforms()
        {
            string[] platforms = {
                "pc", "xbox", "ps", "wii", "android", "ios", "linux"
            };

            foreach (string platformName in platforms)
            {
                Platform platform = new Platform
                {
                    Name = platformName
                };

                Context.Platforms.Add(platform);
            }

            Context.SaveChanges();
        }

        public void GeneratePublishers()
        {
            string[] publishersNames = {
                "bethesda", "microsoft", "obisidan", "bioware", "activision", "larian", "paradox",
                "ubisoft", "blizzard", "valve", "bungie", "sony"
            };

            string[] publishersWebsites = {
                "bethesda-site", "microsoft-site", "obisidan-site", "bioware-site", "activision-site",
                "larian-site", "paradox-site", "ubisoft-site", "blizzard-site", "valve-site"
            };

            for (int i = 0; i < publishersNames.Length; ++i)
            {
                Publisher publisher = new Publisher
                {
                    Name = publishersNames[i]
                };

                if (i < publishersWebsites.Length)
                    publisher.Website = publishersWebsites[i];

                Context.Publishers.Add(publisher);
            }

            Context.SaveChanges();
        }

        public void GenerateReviews()
        {
            Random rand = new Random();

            List<User> users = Context.Users.ToList();
            List<Game> games = Context.Games.ToList();
            List<Language> languages = Context.Languages.ToList();

            string[] descriptionPartOne = {
                "awfull", "excellent", "brilliant", "boring", "the best", "the worst",
                "great",
                "unbeliaveble", "mindblowing", "unexpected", "creative",
                "usual", "unusual", "rare"
            };

            string[] descriptionPartTwo = {
                "experince", "plan", "emotions", "expectations", "idea", "game", "masterpiece",
                "invention", "drama", "comedy"
            };

            string[] descriptionPartThree = {
                "zombie", "friends", "foes", "ghosts", "citizens", "monsters", "animals",
                "rats", "people", "aliens", "creatures",
                "vampires", "murlocs", "elves", "things"
            };

            string[] descriptionPartFour = {
                "killing", "friendship", "history", "help", "life", "saving", "rescue",
                "search", "quests"
            };

            for (int i = 0; i < rand.Next(100); ++i)
            {
                Review review = new Review
                {
                    UserId = users[rand.Next(users.Count)].ID,
                    GameId = games[rand.Next(games.Count)].ID,
                    LanguageId = languages[rand.Next(languages.Count)].ID,

                    Name = descriptionPartThree[rand.Next(descriptionPartThree.Length)] + " of "
                    + descriptionPartTwo[rand.Next(descriptionPartTwo.Length)],

                    Text = descriptionPartOne[rand.Next(descriptionPartOne.Length)]
                    + " " + descriptionPartTwo[rand.Next(descriptionPartTwo.Length)] + " of "
                    + descriptionPartThree[rand.Next(descriptionPartThree.Length)] + " "
                    + descriptionPartFour[rand.Next(descriptionPartFour.Length)],

                    IsRecommend = rand.Next(2) > 0
                };

                Context.Reviews.Add(review);
            }

            Context.SaveChanges();
        }

        public void GeneratUsers()
        {
            Random rand = new Random();

            string[] descriptionPartOne = {
                "awfull", "excellent", "brilliant", "boring", "the best", "the worst",
                "great",
                "unbeliaveble", "mindblowing", "unexpected", "creative",
                "usual", "unusual", "rare"
            };

            string[] descriptionPartThree = {
                "zombie", "friends", "foes", "ghosts", "citizens", "monsters", "animals",
                "rats", "people", "aliens", "creatures",
                "vampires", "murlocs", "elves", "things"
            };

            for (int i = 0; i < rand.Next(100); ++i)
            {
                User user = new User
                {
                    Name = descriptionPartOne[rand.Next(descriptionPartOne.Length)] + " "
                    + " " + descriptionPartThree[rand.Next(descriptionPartThree.Length)],

                    RegistrationDate = DateTime.Now.AddDays(-rand.Next(6000))
                };

                Context.Users.Add(user);
            }

            Context.SaveChanges();
        }

        public void GenerateAll()
        {
            GenerateDevelopers();
            GeneratePublishers();
            GenerateLanguages();
            GenerateGenres();
            GeneratePlatforms();
            GeneratUsers();
            GenerateGames();
            GenerateReviews();
            GenerateGameGenres();
            GenerateGamePlatforms();
        }
    }
}
