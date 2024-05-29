using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("student")]
    public class Student
    {
        public int Id { get; set; }
        public string Sno { get; set; }
        public string Sname { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Tel { get; set; }
        [Column("u_id")]
        public int UId { get; set; }
        [Column("m_id")]
        public int MId { get; set; }
        public int Class { get; set; }
        [Column("d_id")]
        public int DId { get; set; }
        public string Password { get; set; }
        [Column("r_id")]
        public int RId { get; set; }
    }
}
