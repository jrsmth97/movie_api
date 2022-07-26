using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace movie_api.Models
{
    [Table(name: "movie_schedules")]
    public class MovieSchedules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required, Column(name: "movie_id", TypeName = "int")]
        public int movie_id { get; set; }

        [Required, Column(name: "studio_id", TypeName = "int")]
        public int studio_id { get; set; }

        [Required, Column(name: "start_time", TypeName = "varchar(255)")]
        public string start_time { get; set; }

        [Required, Column(name: "end_time", TypeName = "varchar(255)")]
        public string end_time { get; set; }

        [Required, Column(name: "price", TypeName = "double")]
        public double price { get; set; }

        [Required, Column(name: "date", TypeName = "datetime")]
        public DateTime date { get; set; }

        [Required, Column(name: "created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(name: "updated_at", TypeName = "datetime")]
        public DateTime? updated_at { get; set; }

        [Column(name: "deleted_at", TypeName = "datetime")]
        public DateTime? deleted_at { get; set; }

        [ForeignKey("movie_id")]
        public Movies movie { get; set; }

        [ForeignKey("studio_id")]
        public Studios studio { get; set; }
    }
}