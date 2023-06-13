using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_ecommer.Models
{
    [Table ("purchase_order") ]
    public class PurchaseOrder
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("vendor_id")]
        public int VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }

        [NotMapped]
        public string VendorName { get; set; }

        [Column("created_date")]
        public DateTime? CreatedDate { get; set; }

        [Column("total_amount")]
        public Decimal? TotalAmount { get; set; }

        public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

        [NotMapped]
        public int PurchaseOrderItemCount => PurchaseOrderItems?.Count?? 1;

    }
}
