using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("game_platforms")]
    public class GamePlatform
    {
        [Column("game_id")]
        [Required]
        private int gameId;
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        [Column("genre_id")]
        [Required]
        private int platformId;
        public int PlatformId
        {
            get { return platformId; }
            set { platformId = value; }
        }

        [ForeignKey("GameId")]
        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        [ForeignKey("PlatformId")]
        private Platform platform;
        public Platform Platform
        {
            get { return platform; }
            set { platform = value; }
        }
    }
}
