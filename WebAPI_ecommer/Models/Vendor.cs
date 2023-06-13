using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_ecommer.Models
{
    [Table("vendor")]
    public class Vendor
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column("vendor_name")]
        [MaxLength(100)]
        public string VendorName { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("website")]
        [Url]
        public string Website{ get; set; }

        [NotMapped]
        public int ProductCount { get; set; }

        [NotMapped]
        public DateTime LastUpdated { get; set; }

       
    }
}
