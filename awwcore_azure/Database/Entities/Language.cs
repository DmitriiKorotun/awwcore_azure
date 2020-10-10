using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace awwcore_azure.Database.Entities
{
    [Table("languages")]
    public class Language : BaseEntity
    {
        [Column("id")]
        private int id;
        public int ID 
        { 
            get { return id; }
            set { id = value; }
        }

        [Column("name")]
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
