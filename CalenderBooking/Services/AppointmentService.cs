using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalenderBooking.Models;
using CalenderBooking.Repositories;


namespace CalenderBooking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task AddAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.AddAppointmentAsync(appointment);
        }

        public async Task DeleteAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.DeleteAppointmentAsync(appointment);
        }

        public async Task<List<TimeSlot>> GetFreeTimeSlotsAsync(DateTime date)
        {
            return await _appointmentRepository.GetFreeTimeSlotsAsync(date);
        }
        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAppointmentsByDateAsync(date);
        }
        public async Task KeepTimeSlotAsync(TimeSlot timeSlot)
        {
              // Validate the input time slot
            if (timeSlot == null)
            {
                throw new ArgumentNullException(nameof(timeSlot));
            }

            if (timeSlot.StartTime.Hour < 9 || timeSlot.StartTime.Hour >= 17)
            {
                throw new ArgumentException("Time slot must be between 9 AM and 5 PM.");
            }

            // Check if the time slot is already booked
            var existingAppointment = await _appointmentRepository.GetAppointmentAsync(timeSlot.StartTime);
            if (existingAppointment != null)
            {
                throw new ArgumentException("The requested time slot is already booked.");
            }

            // Find the first available date for the time slot
            DateTime currentDate = DateTime.Today;
            while (true)
            {
                var freeTimeSlots = await _appointmentRepository.GetFreeTimeSlotsAsync(currentDate);
                var availableTimeSlot = freeTimeSlots.FirstOrDefault(ts => ts.StartTime.TimeOfDay == timeSlot.StartTime.TimeOfDay);
                if (availableTimeSlot != null)
                {
                    // Reserve the time slot
                    var appointment = new Appointment
                    {
                        DateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, availableTimeSlot.StartTime.Hour, availableTimeSlot.StartTime.Minute, 0)
                        
                    };
                    await _appointmentRepository.AddAppointmentAsync(appointment);
                    Console.WriteLine($"Time slot from {availableTimeSlot.StartTime.ToString("HH:mm")} to {availableTimeSlot.StartTime.Add(TimeSpan.FromMinutes(30)).ToString("HH:mm")} has been kept.");
                    return;
                }

                // Move to the next day
                currentDate = currentDate.AddDays(1);
            }

                }

      
    }       
}
