using System.ComponentModel.DataAnnotations.Schema;

namespace dms.Models
{
    [Table("admin")]
    public class Admin
    {
        public int Id { get; set; }
        public string Aname { get; set; }
        public string Password { get; set; }
        public string Tel { get; set; }
        public int Power { get; set; }
    }
}
