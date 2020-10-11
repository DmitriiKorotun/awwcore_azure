using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("languages")]
    public class Language : BaseEntity
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
        [Required]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
