using Microsoft.Extensions.Configuration;
using movie_api.Models;
using System;

namespace movie_api.Helpers
{
    public class AppHelper
    {
        private readonly IConfiguration _configuration;

        public AppHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Users AvatarInterceptor(Users user)
        {
            if (user.avatar != null) 
                return user;

            string name = Uri.EscapeUriString(user.name);
            string defaultAvatarUrl = _configuration.GetValue<string>("Service:AvatarUrl") + $"?name={name}";
            user.avatar = defaultAvatarUrl;
            return user;
        }

    }
}