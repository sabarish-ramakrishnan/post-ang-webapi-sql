using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using post_ang_webapi_sql.Models;
using post_ang_webapi_sql.Services;

namespace post_ang_webapi_sql.Controllers
{
    [Route("api/[controller]/")]
    public class PostsController : ControllerBase
    {
        public PostService _postService { get; }
        private readonly ILogger _logger;
        private readonly ILoggerManager _nlogger;
        public PostsController(PostService PService, ILogger<PostsController> logger, ILoggerManager nlogger)
        {
            _postService = PService;
            _logger = logger;
            _nlogger = nlogger;
        }

        [HttpGet("")]
        public ActionResult Get()
        {
            _nlogger.LogInfo("Here is info message from our values controller.");
            _nlogger.LogDebug("Here is debug message from our values controller.");
            _nlogger.LogWarn("Here is warn message from our values controller.");
            _nlogger.LogError("Here is error message from our values controller.");
            //int a = int.Parse("asdf");
            _logger.LogInformation("Getting all posts", DateTime.Now.ToString());
            _logger.LogWarning("Getting all posts", DateTime.Now.ToString());
            return Ok(_postService.GetAllPosts());
        }

        [HttpGet("getallposts/{userId}")]
        public ActionResult GetAllPosts(int userId)
        {
            return Ok(_postService.GetAllPostsByUser(userId));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_postService.GetPostById(id));
        }

        [Authorize]
        [HttpPost]
        public ActionResult Post([FromBody] Post newPost)
        {
            return Ok(_postService.Add(newPost));
        }

        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Post newPost)
        {
            _postService.Update(id, newPost);
            return Ok(new ApiResponseModel { Data = newPost });
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Post post = _postService.GetPostById(id);
            if (post == null)
            {
                return NotFound(new ApiResponseModel { Status = "Failed" });
            }
            _postService.Delete(post);
            return Ok(new ApiResponseModel());
        }
    }
}