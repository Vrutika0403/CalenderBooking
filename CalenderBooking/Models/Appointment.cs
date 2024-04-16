using System;
using Microsoft.EntityFrameworkCore;

namespace CalenderBooking.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan Duration {get; } = TimeSpan.FromMinutes(30);

    }
}