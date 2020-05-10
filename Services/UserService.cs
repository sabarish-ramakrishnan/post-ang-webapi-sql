using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using post_ang_webapi_sql.DAL;
using post_ang_webapi_sql.Models;

namespace post_ang_webapi_sql.Services
{
    public class UserService
    {
        private BlogDBContext _dbContext { get; }
        private readonly ILogger _logger;

        public UserService(BlogDBContext dbContext, ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IEnumerable<User> GetAllUsers()
        {
            _logger.LogInformation("_dbContext.Users.Count().ToString()");
            _logger.LogInformation("_dbContext.Users.Count().ToString()");
            return _dbContext.Users;
        }

        public User FindUser(string email, string password)
        {

            return _dbContext.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
        }

        public User FindUserByEmail(string email)
        {

            return _dbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User Add(User newUser)
        {
            newUser.UserRoleId = 1;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
            return newUser;
        }

        public string FindUserRole(int roleId)
        {
            string userRole = string.Empty;
            UserRole role = _dbContext.UserRoles.FirstOrDefault(x => x.Id == roleId);
            if (role != null)
            {
                userRole = role.Title;
            }
            return userRole;
        }

        public string CreateToken(User user, int expiresInSeconds, string secretKey)
        {
            string token = string.Empty;

            try
            {
                var claims = new []
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, FindUserRole(user.UserRoleId)),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var tokenDescriptor = new JwtSecurityToken(
                    issuer: "yourdomain.com",
                    audience: "yourdomain.com",
                    claims : claims,
                    expires : System.DateTime.Now.AddSeconds(expiresInSeconds),
                    signingCredentials : creds);
                token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            }
            catch (System.Exception)
            {
                throw;
            }
            return token;
        }
    }
}