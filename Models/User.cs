using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace post_ang_webapi_sql.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
    }
}