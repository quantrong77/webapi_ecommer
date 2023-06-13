using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebAPI_ecommer.Models;

namespace WebAPI_ecommer.Data
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("user_id")]
        public int User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public User User { get; set; }

        [Column("token")]
        public string Token { get; set; }

        [Column("refresh_tk")]
        public string Refresh_Token { get; set; }

        [Column("jwt_id")]
        public string Jwt_Id { get; set; }

        [Column("is_used")]
        public bool Is_Used { get; set; }

        [Column("is_revoked")] 
        public bool Is_Revoked { get; set; }

        [Column("isused_at")]
        public DateTime Isused_At { get; set; }

        [Column("expried")]
        public DateTime Expried { get; set; }


    }
}
