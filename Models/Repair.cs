using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    public class Repair
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string StudentName { get; set; }

        [Required]
        [StringLength(20)]
        public string StudentNo { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime SubmitTime { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
