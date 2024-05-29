namespace dms.Models
{
    public class InOut
    {
        public int Id { get; set; }
        public int DId { get; set; }
        public int SId { get; set; }
        public DateTime OutDate { get; set; }
        public DateTime? InDate { get; set; }
    }
}
