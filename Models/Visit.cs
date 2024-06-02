using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("visit")]
    public class Visit
    {
        public int Id { get; set; }
        [Column("d_id")]
        public int DId { get; set; }
        [Column("s_id")]
        public int SId { get; set; }
        public string Name { get; set; }
        [Column("in_date")]
        public DateTime InDate { get; set; }
        [Column("out_date")]
        public DateTime? OutDate { get; set; }
    }
}
