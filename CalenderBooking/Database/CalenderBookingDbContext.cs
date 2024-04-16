using Microsoft.EntityFrameworkCore;
using CalenderBooking.Models;

namespace CalenderBooking.Database
{
    public class CalenderBookingDbContext : DbContext
    {
        public CalenderBookingDbContext(DbContextOptions<CalenderBookingDbContext> options)
        : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
    }
}