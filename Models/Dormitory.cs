using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("dormitory")] // 明确指定表名
    public class Dormitory
    {
        [Key]
        public int Id { get; set; }

        [Column("a_id")]
        public int AId { get; set; }

        [Column("ch")]
        public char Ch { get; set; }

        [Column("num")]
        public int Num { get; set; }

        [Column("s_gender")]
        public string SGender { get; set; }
    }
}
