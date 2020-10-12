using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("games")]
    public class Game
    {        
        private int id;
        [Column("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int publisherId;
        [Column("publisher_id")]
        [Required]
        public int PublisherId
        {
            get { return publisherId; }
            set { publisherId = value; }
        }

        private int developerId;
        [Column("developer_id")]
        [Required]
        public int DeveloperId
        {
            get { return developerId; }
            set { developerId = value; }
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

        private string description;
        [Column("description")]
        [MaxLength(200)]
        [Required]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime releaseDate;
        [Column("release_date")]
        [Required]
        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set { releaseDate = value; }
        }

        private DateTime announcementDate;
        [Column("announcment_date")]
        [Required]
        public DateTime AnnouncementDate
        {
            get { return announcementDate; }
            set { announcementDate = value; }
        }
       
        private Publisher publisher;
        [ForeignKey("PublisherId")]
        public Publisher Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }
       
        private Developer developer;
        [ForeignKey("DeveloperId")]
        public Developer Developer
        {
            get { return developer; }
            set { developer = value; }
        }

        public List<GameGenre> GameGenres { get; set; }
        public List<GamePlatform> GamePlatforms { get; set; }
    }
}
