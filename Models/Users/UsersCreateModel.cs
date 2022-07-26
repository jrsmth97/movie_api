using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace movie_api.Models
{
    public class UsersCreate
    {
        [Required]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        public string avatar { get; set; }
    }
}
