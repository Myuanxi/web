using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("in_out")]
    public class InOut
    {
        public int Id { get; set; }

        [Column("d_id")]
        public int DId { get; set; }

        [Column("s_id")]
        public int SId { get; set; }

        [Column("out_date")]
        public DateTime OutDate { get; set; }

        [Column("in_date")]
        public DateTime? InDate { get; set; }
    }
}
