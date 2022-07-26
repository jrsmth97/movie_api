namespace movie_api.Models
{
    public class Login
    {
        public string email { get; set; }
        public string password { get; set; }
        public int is_admin { get; set; }

    }
}