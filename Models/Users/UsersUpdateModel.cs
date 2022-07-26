using System.ComponentModel.DataAnnotations;

namespace movie_api.Models
{
    public class UsersUpdate
    {
        public string name { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public string password { get; set; }

        public string avatar { get; set; }
    }
}
