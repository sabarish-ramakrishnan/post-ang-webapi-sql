using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace post_ang_webapi_sql.Models
{
    [Table("UserRoles")]
    public class UserRole
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }
    }
}