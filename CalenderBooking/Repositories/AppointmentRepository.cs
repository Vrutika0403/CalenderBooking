
using Microsoft.EntityFrameworkCore;
using CalenderBooking.Models;
using CalenderBooking.Database;

namespace CalenderBooking.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly CalenderBookingDbContext _dbContext;

        public AppointmentRepository(CalenderBookingDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Appointment> GetAppointmentAsync(DateTime dateTime)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await _dbContext.Appointments.FirstOrDefaultAsync(a => a.DateTime == dateTime);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<List<TimeSlot>> GetFreeTimeSlotsAsync(DateTime date)
        {
            var appointments = await _dbContext.Appointments.Where(a => a.DateTime.Date == date.Date).ToListAsync();
           
           
            var freeSlots = new List<TimeSlot>();

            var startTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            var endTime = new DateTime(date.Year, date.Month, date.Day, 17, 0, 0);

            while (startTime < endTime)
            {
                if (IsTimeSlotAvailable(appointments, startTime))
                {
                    freeSlots.Add(new TimeSlot { StartTime = startTime });
                }
                startTime = startTime.AddMinutes(30);
            }

            return freeSlots; 
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            _dbContext.Appointments.Add(appointment);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime dateTime)
        {
            return await _dbContext.Appointments.Where(a => a.DateTime == dateTime).ToListAsync();
        }
        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
           
             _dbContext.Appointments.Remove(appointment);
            await _dbContext.SaveChangesAsync();
        }
                
        private bool IsTimeSlotAvailable(List<Appointment> appointments, DateTime startTime)
        {
           
            //Check if timeslot is not between  9AM to 5 PM
            if(startTime.Hour < 9 || startTime.Hour >= 17)
            {
                return false;
            }

            // Check if the time slot is during the reserved time (4 PM to 5 PM on each second day of the third week)
            if(startTime.Hour >= 16 && startTime.Hour < 17)
            {
                var weekday = (int)startTime.DayOfWeek;
                var weekmonth = GetWeekOfMonth(startTime);
                if(weekday == 2 && weekmonth == 3)
                {
                    return false;
                }
            }

            //Check if appointment time is already booked 
            foreach(var appointment in appointments)
            {
                if (appointment.DateTime.Date == startTime.Date && appointment.DateTime.TimeOfDay == startTime.TimeOfDay)
                {
                    return false;
                }
            }

            return true;
        }
        private int GetWeekOfMonth(DateTime date)
        {
             var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
             var firstDayOfWeek = firstDayOfMonth.DayOfWeek;

             var daysOffset = date.Day + (int)firstDayOfWeek - 1;
             var weekNumber = 1 + (daysOffset) / 7;

            return weekNumber;
        }
    }
}
