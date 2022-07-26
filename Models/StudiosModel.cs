using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movie_api.Models
{
    [Table(name: "studios")]
    public class Studios
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required, Column(name: "studio_number", TypeName = "int")]
        public int studio_number { get; set; }

        [Required, Column(name: "seat_capacity", TypeName = "int")]
        public int seat_capacity { get; set; }

        [Required, Column(name: "created_at", TypeName = "datetime")]
        public DateTime created_at { get; set; }

        [Column(name: "updated_at", TypeName = "datetime")]
        public DateTime? updated_at { get; set; }
        
        [Column(name: "deleted_at", TypeName = "datetime")]
        public DateTime? deleted_at { get; set; }
    }
}