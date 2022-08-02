using System;
using System.Web;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Helpers;
using movie_api.Attributes;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppHelper _helper;

        public UsersController(
            IUserRepository userRepository,
            ILogger<UsersController> logger,
            IConfiguration configuration
        )
        {
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
            _helper = new AppHelper(_configuration);
        }

        [Authorize(Roles = "1")]
        [HttpGet("/api/users")]
        public async Task<IActionResult> GetUsers()
        {
            IList<Users> users = await _userRepository.GetListAsync();
            _logger.LogInformation("[GET: /api/users] All users data accessed");
            return Ok(users);
        }

        [Authorize]
        [HttpGet("/api/users/my-account")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            Users user = await _userRepository.GetAsync(userId);
            if (user == null) {
                _logger.LogInformation("[GET: /api/users/my-account] user data not found ( id : " + userId + ")");
                return NotFound("User not found");
            }

            _logger.LogInformation("[GET: /api/users/my-account] user data accessed ( id : " + userId + ")");
            return Ok(user);
        }

        [Authorize(Roles = "1")]
        [HttpGet("/api/users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            Users user = await _userRepository.GetAsync(id);
            if (user == null) {
                _logger.LogInformation("[GET: /api/users/{id}] user data not found ( id : " + id + ")");
                return NotFound("User not found");
            }

            _logger.LogInformation("[GET: /api/users/{id}] user data accessed ( id : " + id + ")");
            return Ok(user);
        }

        [Authorize(Roles = "1")]
        [HttpPost("/api/users")]
        public async Task<IActionResult> AddUser(Users user)
        {
            user = _helper.AvatarInterceptor(user);
            user.password =  new PasswordHasher<object>().HashPassword(null, user.password);

            UsersValidation Obj = new UsersValidation();
            ValidationResult Result = Obj.Validate(user);

            if (Result.IsValid == false) {
                return BadRequest(Result);
            }
            
            _logger.LogInformation($"[POST: /api/users] User create => '{JsonConvert.SerializeObject(user)}'");
            Users createUser = await _userRepository.CreateAsync(user);

            return Ok(createUser);
        }
        
        [Authorize]
        [HttpPatch("/api/users/update-avatar")]
        public async Task<IActionResult> UpdateAvatar([FromForm] UsersUpdateAvatar avatar)
        {
            var userId = int.Parse(HttpContext.Items["UserId"].ToString());
            string path = "";
            try
            {
                    if (avatar.avatar_file.Length <= 0) 
                    {
                        return BadRequest("File not found");
                    }

                    string fileName = Guid.NewGuid() + Path.GetExtension(avatar.avatar_file.FileName);
                    path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await avatar.avatar_file.CopyToAsync(fileStream);
                        Users existingUser = await _userRepository.GetAsync(userId);
                        if (existingUser.avatar.Contains("Upload"))
                        {
                            string oldFileName = new String(existingUser.avatar).Replace("/Upload/", "");
                            var oldFilePath = Path.Combine(path, oldFileName);
                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        Users updateUser = await _userRepository.UpdateAsync(userId, new UsersUpdate{ avatar = "/Upload/" + fileName });
                        return Ok(updateUser);
                    }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("/api/users/delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            Users user = await _userRepository.DeleteAsync(id);
           
            if (user == null) 
            {
                _logger.LogInformation("[DELETE: /api/users/delete/{id}] not exist user ( id : " + id + ")");
                return NotFound("User not found");
            }
            
            _logger.LogInformation("[DELETE: /api/users/delete/{id}] Deleting user ( id : "+id+")");
            return Ok(user);

        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/users/delete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            Users user = await _userRepository.SoftDeleteAsync(id);
           
            if (user == null) 
            {
                _logger.LogInformation("[PATCH: /api/users/delete/{id}] not exist user ( id : " + id + ")");
                return NotFound("User not found");
            }
            
            _logger.LogInformation("[PATCH: /api/users/delete/{id}] Deleting user ( id : "+id+")");
            return Ok(user);
        }

        [Authorize(Roles = "1")]
        [HttpPatch("/api/users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UsersUpdate user)
        {
            if (user.password != null)
                user.password = new PasswordHasher<object>().HashPassword(null, user.password);
                
            Users updateUser = await _userRepository.UpdateAsync(id, user);
             
            if (updateUser == null) 
            {
                _logger.LogInformation("[PATCH: /api/users/{id}] not exist user ( id : " + id + ")");
                return NotFound("User not found");
            }
            
            _logger.LogInformation("[PATCH: /api/users/{id}] Updating user ( id : "+ id +")");
            return Ok(updateUser);
        }
    }
}