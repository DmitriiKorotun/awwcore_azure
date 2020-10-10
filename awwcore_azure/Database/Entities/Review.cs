using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("reviews")]
    public class Review
    {
        [Column("id")]
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column("user_id")]
        [Required]
        private int userId;
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [Column("game_id")]
        [Required]
        private int gameId;
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        [Column("language_id")]
        [Required]
        private int languageId;
        public int LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        [Column("name")]
        [Required]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("text")]
        [Required]
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        [Column("is_recommend")]
        [Required]
        private bool isRecommend;
        public bool IsRecommend
        {
            get { return isRecommend; }
            set { isRecommend = value; }
        }

        [ForeignKey("UserId")]
        private User user;
        public User User
        {
            get { return user; }
            set { user = value; }
        }

        [ForeignKey("GameId")]
        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        [ForeignKey("LanguageId")]
        private Language language;
        public Language Language
        {
            get { return language; }
            set { language = value; }
        }
    }
}
