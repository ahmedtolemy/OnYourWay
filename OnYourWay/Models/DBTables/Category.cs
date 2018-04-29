using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnYourWay.Models.DBTables
{
    public class Category
    {
        public Category()
        {
            this.Clients =new HashSet<Client>();
        }
        [Key]
        public int ID { get; set; }
        public string NameAR { get; set; }
        public string NameEN { get; set; }
        public string NameOR { get; set; }
        public bool? Delete { get; set; }
        public bool? Block { get; set; }
        public int? CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category category { get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Client> Clients { get; set; }
    }
}