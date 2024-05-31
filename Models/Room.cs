using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("room")]
    public class Room
    {
        public int Id { get; set; }

        [Column("d_id")]
        public int DId { get; set; }

        public int Num { get; set; }

        public bool Margin { get; set; }

        public int Price { get; set; }
    }
}
