using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CalenderBooking.Models;
using CalenderBooking.Repositories;
using CalenderBooking.Services;
using CalenderBooking.Database;
using Microsoft.EntityFrameworkCore.InMemory;
using CalenderBooking;
using Microsoft.EntityFrameworkCore;

namespace test_calenderbooking
{

public class AppointmentRepoTest
{
   
        [Fact]
        public async Task GetFreeTimeSlotsAsync_ShouldReturnAvailableTimeSlots()
        {
            // Arrange
           var options = new DbContextOptionsBuilder<CalenderBookingDbContext>()
       .UseInMemoryDatabase(databaseName: "TestDatabase")
       .Options;


            using (var context = new CalenderBookingDbContext(options))
            {
                var repository = new AppointmentRepository(context);

                // Add some appointments to the database
                await context.Appointments.AddRangeAsync(
                    new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) },
                    new Appointment { DateTime = new DateTime(2023, 4, 15, 11, 0, 0) });
                await context.SaveChangesAsync();

                // Act
                var freeTimeSlots = await repository.GetFreeTimeSlotsAsync(new DateTime(2023, 4, 15));

                // Assert
                Assert.Equal(10, freeTimeSlots.Count);
            }
        }
        [Fact]
public async Task AddAppointmentAsync_ShouldThrowExceptionForInvalidAppointment()
{
    // Arrange
    var appointmentService = new AppointmentService(null);

    // Act and Assert
    await Assert.ThrowsAsync<ArgumentException>(() => appointmentService.AddAppointmentAsync(null));
}
[Fact]
public async Task KeepTimeSlotAsync_ShouldKeepTimeSlot()
{
    // Arrange
    var timeSlot = new TimeSlot { StartTime = DateTime.Now };
    var appointmentService = new AppointmentService(null);

    // Act
    await appointmentService.KeepTimeSlotAsync(timeSlot);

    // Assert
    // Add assertions here to verify that the time slot was kept
}
}

}
      