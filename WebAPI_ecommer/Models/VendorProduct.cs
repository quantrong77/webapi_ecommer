using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WebAPI_ecommer.Models
{
    [Table("vendor_product")]
    public class VendorProduct
    {
        [Key]
        [Column("vendor_id")]
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }

        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("quantity_supply")]
        public int QuantitySupply { get; set; }

        [Column("time_supply")]
        public DateTime TimeSupply { get; set; }
    }
}
