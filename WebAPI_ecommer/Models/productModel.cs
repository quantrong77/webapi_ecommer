using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_ecommer.Models
{
    public class productModel
    {
       
        public int Id { get; set; }

      
        public string Sku { get; set; }

       
        public string Name { get; set; }

        
        public string Description { get; set; }

        
        public double UnitPrice { get; set; }

        
        public string ImageUrl { get; set; }

      
        public bool Active { get; set; }

       
        public int UnitsInStock { get; set; }

       
        public DateTime DateCreated { get; set; }

        
        public DateTime LastUpdated { get; set; }

        [Required]
        public int CategoryId { get; set; }
       
    }
}
