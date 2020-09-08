using Microsoft.EntityFrameworkCore;
using Pjx.CalendarEntity.Models;

namespace Pjx_Api.Data
{
    public class CalendarDbContext: DbContext
    {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options) { }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Department> Departments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEvent>()
                .ToTable("CalendarEvents")
                .HasIndex(b => b.UserId)
                .HasName("Index_UserId");

            modelBuilder.Entity<Organization>()
                .HasIndex(b => b.OrganizationId)
                .HasName("Index_OrganizationId");

            modelBuilder.Entity<Department>()
                .HasIndex(b => b.DepartmentId)
                .HasName("Index_DepartmentId");
        }
    }
}
