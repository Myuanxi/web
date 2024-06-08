using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("Reservations")]
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Venue { get; set; }

        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Student Student { get; set; }
    }
}
