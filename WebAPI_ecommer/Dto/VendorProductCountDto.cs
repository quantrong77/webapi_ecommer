using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_ecommer.Dto
{
    public class VendorProductCountDto
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int ProductCount { get; set; }
    }
}
