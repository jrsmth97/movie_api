using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using movie_api.Contexts;

namespace movie_api.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly DBContext _db;
        
        public AuthMiddleware(RequestDelegate next, IConfiguration configuration, DBContext db)
        {
            _next           = next;
            _configuration  = configuration;
            _db             = db;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault();
            if (token != null) 
                injectAccount(context, token);

            await _next(context);
        }

        private async void injectAccount(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secret = _configuration.GetValue<string>("Service:Secret");
                var key = Encoding.ASCII.GetBytes(secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    RequireExpirationTime = true,  
                    ValidateIssuer = false,  
                    ValidateAudience = false,  
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;
                var userRole = jwtToken.Claims.First(x => x.Type == "role").Value;
  
                context.Items["UserId"] = userId;
                context.Items["UserRole"] = userRole;
                context.Items["UserEmail"] = userEmail;
            }
            catch
            {
                await _next(context);
            }
        }

    }
}