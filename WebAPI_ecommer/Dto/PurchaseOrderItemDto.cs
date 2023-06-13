using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_ecommer.Dto
{
    public class PurchaseOrderItemDto
    {
        public int Id { get; set; }
        //public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        //public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
       
    }
}
