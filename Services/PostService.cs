using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using post_ang_webapi_sql.DAL;
using post_ang_webapi_sql.Models;

namespace post_ang_webapi_sql.Services
{
    public class PostService
    {
        private BlogDBContext _dbContext { get; }
        private readonly ILogger _logger;
        public PostService(BlogDBContext dbContext, ILogger<PostService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IEnumerable<Post> GetAllPosts()
        {
            _logger.LogInformation("_dbContext.Users.Count().ToString()");
            _logger.LogInformation("_dbContext.Users.Count().ToString()");
            return _dbContext.Posts;
        }

        public IEnumerable<Post> GetAllPostsByUser(int userId)
        {
            return _dbContext.Posts.Where(x => x.UserId == userId);
        }

        public Post GetPostById(int id)
        {
            return _dbContext.Posts.FirstOrDefault(x => x.Id == id);
        }

        public Post Add(Post newPost)
        {
            _dbContext.Posts.Add(newPost);
            _dbContext.SaveChanges();
            return newPost;
        }

        public void Update(int id, Post newPost)
        {
            _dbContext.Posts.Update(newPost);
            _dbContext.SaveChanges();
            //return newPost;
        }

        public void Delete(Post post)
        {
            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
            //return newPost;
        }
    }
}