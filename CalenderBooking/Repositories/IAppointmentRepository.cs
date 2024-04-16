using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalenderBooking.Models;

namespace CalenderBooking.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointmentAsync(DateTime dateTime);
        Task<List<TimeSlot>> GetFreeTimeSlotsAsync(DateTime date);
        Task AddAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Appointment appointment);
    }
}
