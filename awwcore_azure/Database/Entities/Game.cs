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
        [Column("id")]
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column("publisher_id")]
        [Required]
        private int publisherId;
        public int PublisherId
        {
            get { return publisherId; }
            set { publisherId = value; }
        }

        [Column("developer_id")]
        [Required]
        private int developerId;
        public int DeveloperId
        {
            get { return developerId; }
            set { developerId = value; }
        }

        [Column("name")]
        [Required]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [Column("description")]
        [Required]
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [Column("release_date")]
        [Required]
        private DateTime releaseDate;
        public DateTime ReleaseDate
        {
            get { return releaseDate; }
            set { releaseDate = value; }
        }

        [Column("announcement_date")]
        [Required]
        private DateTime announcementDate;
        public DateTime AnnouncementDate
        {
            get { return announcementDate; }
            set { announcementDate = value; }
        }

        [ForeignKey("PublisherId")]
        private Publisher publisher;
        public Publisher Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        [ForeignKey("DeveloperId")]
        private Developer developer;
        public Developer Developer
        {
            get { return developer; }
            set { developer = value; }
        }
    }
}
