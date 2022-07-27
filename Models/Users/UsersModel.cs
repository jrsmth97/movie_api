using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_api.Models
{
    [Table(name:"users")]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required, Column(name: "name", TypeName = "varchar(255)")]
        public string name { get; set; }

        [EmailAddress]
        [Required, Column(name: "email", TypeName = "varchar(255)")]
        public string email { get; set; }

        [Required, Column(name: "password", TypeName = "varchar(255)")]
        public string password { get; set; }

        [Column(name: "avatar", TypeName = "varchar(255)")]
        public string avatar { get; set; }

        [Required, Column(name: "is_admin", TypeName = "int")]
        public int is_admin { get; set; }

        [Column(name: "is_confirmed", TypeName = "int")]
        public int is_confirmed { get; set; }

        [Column(name: "activation_key", TypeName = "varchar(255)")]
        public string activation_key { get; set; }

        [Required, Column(name: "created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(name: "updated_at", TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(name: "deleted_at", TypeName = "datetime")]
        public DateTime? deleted_at { get; set; }

    }
}