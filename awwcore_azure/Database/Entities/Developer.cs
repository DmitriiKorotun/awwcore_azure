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
        [Column("id")]
        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [Column("name")]
        [Required]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        [Column("website")]
        [Required]
        private string website;
        public string Website
        {
            get { return website; }
            set { website = value; }
        }
    }
}
