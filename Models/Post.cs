using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace post_ang_webapi_sql.Models
{
    [Table("Posts")]
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        [Required]
        [StringLength(500)]
        public string Content { get; set; }
        [Required]
        [StringLength(500)]
        public string ImagePath { get; set; }
        public int UserId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public User User { get; set; }

    }
}