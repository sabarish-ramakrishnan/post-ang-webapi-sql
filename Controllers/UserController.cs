using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using post_ang_webapi_sql.Models;
using post_ang_webapi_sql.Services;

namespace post_ang_webapi_sql.Controllers
{
    [Route("/api/[controller]/")]
    public class UserController : ControllerBase
    {
        public UserService _userService { get; }

        private string _secretKey = string.Empty;
        private int _expiresInSeconds { get; set; }

        public UserController(UserService userService, IConfiguration config)
        {
            _userService = userService;
            _secretKey = Convert.ToString(config["SecretKey"]);
            _expiresInSeconds = Convert.ToInt32(config["expiresInSeconds"]);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            User user = _userService.FindUser(loginUser.Email, loginUser.Password);
            if (user == null)
            {
                return NotFound();
            }
            string token = _userService.CreateToken(user, _expiresInSeconds, _secretKey);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new ApiResponseModel { Status = "Invalid Token" });
            }
            //var responseData = new { expiresIn = 3600, token = token, userId = user.Id };
            return Ok(new ApiResponseModel { Status = "Login successfull", Data = new { token = token } });
        }

        [HttpPost("signup")]
        public IActionResult Signup([FromBody] User loginUser)
        {
            User user = _userService.FindUserByEmail(loginUser.Email);
            if (user != null)
            {
                return BadRequest(new ApiResponseModel { Status = "User already exists", Data = null });
            }
            user = _userService.Add(loginUser);

            var responseData = new { userId = user.Id };
            return Ok(new ApiResponseModel { Status = "Signup successfull", Data = responseData });
        }

    }
}