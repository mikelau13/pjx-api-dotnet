using Microsoft.EntityFrameworkCore;
using Pjx.CalendarLibrary.Models;

namespace Pjx_Api.Data
{
    public class CalendarDbContext: DbContext
    {
        public CalendarDbContext(DbContextOptions<CalendarDbContext> options) : base(options) { }

        public DbSet<CalendarEvent> CalendarEvents { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalendarEvent>()
                .ToTable("CalendarEvents")
                .HasIndex(b => b.UserId)
                .HasName("Index_UserId");
        }
    }
}
