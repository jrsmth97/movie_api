using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace movie_api.Models
{
    [Table(name: "movie_tags")]
    public class MovieTags
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required, Column(name: "movie_id", TypeName = "int")]
        public int movie_id { get; set; }

        [Required, Column(name: "tag_id", TypeName = "int")]
        public int tag_id { get; set; }

        [Required, Column(name: "created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(name: "updated_at", TypeName = "datetime")]
        public DateTime? updated_at { get; set; }
        
        [Column(name: "deleted_at", TypeName = "datetime")]
        public DateTime? deleted_at { get; set; }

        [ForeignKey("tag_id")]
        public Tags tag { get; set; }

        [ForeignKey("movie_id")]
        public Movies movie { get; set; }
    }
}