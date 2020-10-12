using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("developers")]
    public class Developer
    {        
        private int id;
        [Column("id")]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        [Column("name")]
        [MaxLength(75)]
        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
       
        private string website;
        [Column("website")]
        [MaxLength(100)]
        public string Website
        {
            get { return website; }
            set { website = value; }
        }
    }
}
