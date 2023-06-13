using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;






namespace WebAPI_ecommer.Models
{
    public class CategoryModel
    {

        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }

    }
}
