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
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        private int genreId;
        public int GenreId
        {
            get { return genreId; }
            set { genreId = value; }
        }
        
        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }
       
        private Genre genre;
        public Genre Genre
        {
            get { return genre; }
            set { genre = value; }
        }
    }
}
