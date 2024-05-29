namespace dms.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public int DId { get; set; }
        public int SId { get; set; }
        public string Name { get; set; }
        public DateTime InDate { get; set; }
        public DateTime? OutDate { get; set; }
    }
}
