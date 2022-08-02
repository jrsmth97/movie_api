using System;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using movie_api.Attributes;

namespace movie_api.Models
{
    public class UsersUpdateAvatar
    {
        [Required]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        public IFormFile avatar_file {get; set;}
    }
}