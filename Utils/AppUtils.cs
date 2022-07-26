using System;
using System.Linq;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using MailKit.Net.Smtp;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using MimeKit;
using movie_api.Models;

namespace movie_api.Utils
{
    public class UserVerify {
        public string id;
        public string email;
        public string role;
    }

    public class AppUtils {
        
        private IConfiguration _configuration;

        public AppUtils(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public string generateRandomString(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public string generateToken(Users user)
        {
            string secret = _configuration.GetValue<string>("Service:Secret");
            int expireDays = _configuration.GetValue<int>("Service:LoginExpiredDay");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("id", user.id.ToString()),
                    new Claim("email", user.email),
                    new Claim("role", user.is_admin.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(expireDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserVerify verifyToken(string token) 
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

            UserVerify user = new UserVerify();
            user.id = userId;
            user.email = userEmail;
            user.role = userRole;

            return user;
        }

        public void SendEmail(string name, string email, string subject, string messages)
        {
            string MAIL_HOST        = _configuration.GetValue<string>("Service:MailHost");
            int MAIL_PORT           = _configuration.GetValue<int>("Service:MailPort");
            string MAIL_FROM_EMAIL  = _configuration.GetValue<string>("Service:MailFromEmail");
            string MAIL_FROM_NAME   = _configuration.GetValue<string>("Service:MailFromName");
            string MAIL_USER        = _configuration.GetValue<string>("Service:MailUserId");
            string MAIL_PASSWORD    = _configuration.GetValue<string>("Service:MailPassword");
            
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(MAIL_FROM_NAME, MAIL_FROM_EMAIL));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = messages,
            };
            using (var client = new SmtpClient())
            {
                client.Connect(MAIL_HOST, MAIL_PORT, false);
                client.Authenticate(MAIL_USER, MAIL_PASSWORD);

                client.Send(message);
                client.Disconnect(true);
            }
        }

    }
}