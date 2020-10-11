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
        private int gameId;
        [Column("game_id")]
        [Required]
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        private int platformId;
        [Column("genre_id")]
        [Required]
        public int PlatformId
        {
            get { return platformId; }
            set { platformId = value; }
        }
       
        private Game game;
        [ForeignKey("GameId")]
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        private Platform platform;
        [ForeignKey("PlatformId")]
        public Platform Platform
        {
            get { return platform; }
            set { platform = value; }
        }
    }
}
