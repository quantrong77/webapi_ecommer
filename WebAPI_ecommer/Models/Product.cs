using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_ecommer.Models
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("sku")]
        public string Sku { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("unit_price")]
        public double UnitPrice { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        [Column("units_in_stock")]
        public int UnitsInStock { get; set; }

        [Column("date_created")]
        public DateTime? DateCreated { get; set; }

        [Column("last_updated")]
        public DateTime? LastUpdated { get; set; }

        [Required]
        [Column("category_id")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
  
}
