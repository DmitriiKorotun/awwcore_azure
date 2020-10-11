using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("game_genres")]
    public class GameGenre
    {
        private int gameId;
        [Column("game_id")]
        [Required]
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        private int genreId;
        [Column("genre_id")]
        [Required]
        public int GenreId
        {
            get { return genreId; }
            set { genreId = value; }
        }
        
        private Game game;
        [ForeignKey("GameId")]
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }
       
        private Genre genre;
        [ForeignKey("GenreId")]
        public Genre Genre
        {
            get { return genre; }
            set { genre = value; }
        }
    }
}
