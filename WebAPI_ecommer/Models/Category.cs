using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_ecommer.Models
{
    [Table("product_category")]
    public class Category
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("category_name")]
        public string CategoryName { get; set; }

        // thuoc tinh khac cua ProductCategory

        //public virtual ICollection<Product> Products { get; set; }
    }
}
