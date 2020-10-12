using awwcore_azure.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Interface
{
    public class GameReviewsContext : DbContext
    {
        public GameReviewsContext(DbContextOptions<GameReviewsContext> options)
            : base(options) { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameGenre>()
                .HasKey(gg => new { gg.GameId, gg.GenreId });

            modelBuilder.Entity<GamePlatform>()
                .HasKey(gp => new { gp.GameId, gp.PlatformId });

            modelBuilder.Entity<GameGenre>().Property<int>("GameId").HasColumnName("game_id");
            modelBuilder.Entity<GameGenre>().Property<int>("GenreId").HasColumnName("genre_id");

            modelBuilder.Entity<GamePlatform>().Property<int>("GameId").HasColumnName("game_id");
            modelBuilder.Entity<GamePlatform>().Property<int>("PlatformId").HasColumnName("platform_id");

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Game)
                .WithMany(g => g.GameGenres)
                .HasForeignKey(y => y.GameId);

            modelBuilder.Entity<GameGenre>()
    .HasOne(gg => gg.Genre)
    .WithMany(g => g.GameGenres)
    .HasForeignKey(y => y.GenreId);

            modelBuilder.Entity<GamePlatform>()
    .HasOne(gp => gp.Game)
    .WithMany(g => g.GamePlatforms)
    .HasForeignKey(y => y.GameId);

            modelBuilder.Entity<GamePlatform>()
    .HasOne(gp => gp.Platform)
    .WithMany(p => p.GamePlatforms)
    .HasForeignKey(y => y.PlatformId);



            base.OnModelCreating(modelBuilder);
        }
    }
}
