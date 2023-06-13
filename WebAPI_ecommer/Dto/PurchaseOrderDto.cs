using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_ecommer.Dto
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Decimal? TotalAmount { get; set; }

        public List<PurchaseOrderItemDto> PurchaseOrderItems { get; set; }

        public int PurchaseOrderItemCount { get; set; }
    }
}
