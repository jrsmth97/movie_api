using System.ComponentModel.DataAnnotations;

namespace movie_api.Models
{
    public class UserActivation
    {
        [EmailAddress]
        public string email { get; set; }

        public string activation_key { get; set; }

    }
}