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
        private int id;
        [Column("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int userId;
        [Column("user_id")]
        [Required]
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        private int gameId;
        [Column("game_id")]
        [Required]
        public int GameId
        {
            get { return gameId; }
            set { gameId = value; }
        }

        private int languageId;

        [Column("language_id")]
        [Required]
        public int LanguageId
        {
            get { return languageId; }
            set { languageId = value; }
        }

        private string name;
        [Column("name")]
        [MaxLength(50)]
        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string text;
        [Column("text")]
        [MaxLength(1500)]
        [Required]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private bool isRecommend;
        [Column("is_reccommend")]
        [Required]
        public bool IsRecommend
        {
            get { return isRecommend; }
            set { isRecommend = value; }
        }
       
        private User user;
        [ForeignKey("UserId")]
        public User User
        {
            get { return user; }
            set { user = value; }
        }
       
        private Game game;
        [ForeignKey("GameId")]
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        private Language language;
        [ForeignKey("LanguageId")]
        public Language Language
        {
            get { return language; }
            set { language = value; }
        }
    }
}
