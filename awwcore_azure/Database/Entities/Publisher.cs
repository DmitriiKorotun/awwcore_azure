using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("publishers")]
    public class Publisher
    {
        [Column("id")]
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column("name")]
        [MaxLength(75)]
        [Required]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        [Column("website")]
        [MaxLength(100)]
        private string website;
        public string Website
        {
            get { return website; }
            set { website = value; }
        }
    }
}
