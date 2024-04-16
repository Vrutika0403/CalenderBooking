using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalenderBooking.Models;

namespace CalenderBooking.Services
{
    public interface IAppointmentService
    {
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Appointment appointment);
        Task<List<TimeSlot>> GetFreeTimeSlotsAsync(DateTime date);
        Task KeepTimeSlotAsync(TimeSlot timeSlot);
    }
}
