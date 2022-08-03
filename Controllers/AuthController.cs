using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using FluentValidation.Results;
using Newtonsoft.Json;
using movie_api.Models;
using movie_api.Validations;
using movie_api.Repositories;
using movie_api.Utils;
using movie_api.Helpers;

namespace movie_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppUtils _utils;
        private readonly AppHelper _helper;

        public AuthController(
            IUserRepository userRepository,
            ILogger<AuthController> logger,
            IConfiguration configuration
        )
        
        {
            _logger = logger;
            _userRepository = userRepository;
            _configuration = configuration;
            _utils = new AppUtils(_configuration);
            _helper = new AppHelper(_configuration);
        }

        [HttpPost("/api/auth/login/")]
        public async Task<IActionResult> UserLogin(Login login)
        {
            Users user = await _userRepository.GetLoginAsync(login);
            if (user == null) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.LOGIN_FAILED,
                    new string[] { "Invalid login credentials" },
                    StatusCodes.Status400BadRequest
                ));
            }

            if (user.is_confirmed == 0) {
                return Unauthorized(ResponseBuilder.FailedResponse(
                    ResponseBuilder.LOGIN_FAILED,
                    new string[] { "Please activate your account" },
                    StatusCodes.Status401Unauthorized
                ));
            }

            PasswordVerificationResult verifyPassword = new PasswordHasher<object>().VerifyHashedPassword(null, user.password, login.password);
            if (verifyPassword == PasswordVerificationResult.Failed) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.LOGIN_FAILED,
                    new string[] { "Invalid login credentials" },
                    StatusCodes.Status400BadRequest
                ));
            }

            _logger.LogInformation($"[POST: /api/auth/login] User login requested => '{JsonConvert.SerializeObject(user)}'");
            var token = _utils.generateToken(user);
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.LOGIN_OK,
                new { token = token }
            ));
        }

        
        [HttpPost("/api/auth/user-activation")]
        public async Task<IActionResult> UserActivation(UserActivation activation)
        {
            Users user = await _userRepository.GetActivateAsync(activation);

            if (user == null) {
                return NotFound(ResponseBuilder.FailedResponse(
                    ResponseBuilder.ACCOUNT_ACTIVATION_FAILED,
                    new string[] { "Email or Activation code not valid" },
                    StatusCodes.Status404NotFound
                ));
            }

            _logger.LogInformation("[POST: /api/auth/user-activation] Activating user data ( email : "+ user.email +")");
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.ACCOUNT_ACTIVATION_OK
            ));
        }

        [HttpPost("/api/auth/register")]
        public async Task<IActionResult> UserRegister(Users user)
        {
            user = _helper.AvatarInterceptor(user);

            user.activation_key = _utils.generateRandomString(10);
            user.password =  new PasswordHasher<object>().HashPassword(null, user.password);

            UsersValidation Obj = new UsersValidation();
            ValidationResult Result = Obj.Validate(user);

            if (Result.IsValid == false) {
                return BadRequest(ResponseBuilder.FailedResponse(
                    ResponseBuilder.LOGIN_FAILED,
                    Result,
                    StatusCodes.Status400BadRequest
                ));
            }
            
            _logger.LogInformation($"[POST: /api/auth/register] User creation requested => '{JsonConvert.SerializeObject(user)}'");
            string subject = "Activation Code For Movie Api";
            string messages = $"Your activation code is : {user.activation_key}";
            Thread sendMail = new Thread(() => _utils.SendEmail(user.name, user.email, subject, messages));
            sendMail.Start();

            Users createUser = await _userRepository.CreateAsync(user);
            return Ok(ResponseBuilder.SuccessResponse(
                ResponseBuilder.REGISTER_OK
            ));
        }
    }
}