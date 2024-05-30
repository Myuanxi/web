using Microsoft.EntityFrameworkCore;

namespace dms.Models
{
    public class DmsContext : DbContext
    {
        public DmsContext(DbContextOptions<DmsContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Dormitory> Dormitories { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Major> Majors { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<InOut> InOuts { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
