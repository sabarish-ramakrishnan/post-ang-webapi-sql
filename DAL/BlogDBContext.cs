using Microsoft.EntityFrameworkCore;
using post_ang_webapi_sql.Models;

namespace post_ang_webapi_sql.DAL {
    public class BlogDBContext : DbContext {
        public BlogDBContext(DbContextOptions<BlogDBContext> options) : base(options) {

        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }
    }

}