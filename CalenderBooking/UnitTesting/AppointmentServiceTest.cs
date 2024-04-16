using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using CalenderBooking.Models;
using CalenderBooking.Repositories;
using CalenderBooking.Services;

namespace CalenderBooking.UnitTesting
{
    public class AppointmentServiceTests
    {
        [Fact]
        public async Task AddAppointmentAsync_ShouldAddAppointment()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var appointment = new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) };

            // Act
            await appointmentService.AddAppointmentAsync(appointment);

            // Assert
            mockRepository.Verify(r => r.AddAppointmentAsync(appointment), Times.Once);
        }
        [Fact]
        public async Task DeleteAppointmentAsync_ShouldDeleteAppointment()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var appointment = new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) };

            // Act
            await appointmentService.DeleteAppointmentAsync(appointment);

            // Assert
            mockRepository.Verify(r => r.DeleteAppointmentAsync(appointment), Times.Once);
        }

        [Fact]
        public async Task GetFreeTimeSlotsAsync_ShouldReturnFreeTimeSlots()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var date = new DateTime(2023, 4, 15);
            var freeTimeSlots = new List<TimeSlot>
            {
                new TimeSlot { StartTime = new DateTime(2023, 4, 15, 9, 0, 0) },
                new TimeSlot { StartTime = new DateTime(2023, 4, 15, 9, 30, 0) },
                new TimeSlot { StartTime = new DateTime(2023, 4, 15, 10, 0, 0) }
            };

            mockRepository.Setup(r => r.GetFreeTimeSlotsAsync(date)).ReturnsAsync(freeTimeSlots);

            // Act
            var result = await appointmentService.GetFreeTimeSlotsAsync(date);

            // Assert
            Assert.Equal(freeTimeSlots, result);
            mockRepository.Verify(r => r.GetFreeTimeSlotsAsync(date), Times.Once);
        }

        [Fact]
        public async Task KeepTimeSlotAsync_ShouldKeepTimeSlot()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var timeSlot = new TimeSlot { StartTime = new DateTime(2023, 4, 15, 9, 0, 0) };

            // Act
            await appointmentService.KeepTimeSlotAsync(timeSlot);

            
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldThrowExceptionForInvalidAppointment()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var appointment = new Appointment { DateTime = new DateTime(2023, 4, 14, 10, 0, 0) };

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => appointmentService.AddAppointmentAsync(appointment));
            mockRepository.Verify(r => r.AddAppointmentAsync(appointment), Times.Never);
        }

        [Fact]
        public async Task GetAppointmentsByDateAsync_ShouldReturnAppointments()
        {
            // Arrange
            var mockRepository = new Mock<IAppointmentRepository>();
            var appointmentService = new AppointmentService(mockRepository.Object);
            var date = new DateTime(2023, 4, 15);
            var appointments = new List<Appointment>
            {
                new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) },
                new Appointment { DateTime = new DateTime(2023, 4, 15, 11, 0, 0) }
            };

            mockRepository.Setup(r => r.GetAppointmentsByDateAsync(date)).ReturnsAsync(appointments);

            // Act
            var result = await appointmentService.GetAppointmentsByDateAsync(date);

            // Assert
            Assert.Equal(appointments, result);
            mockRepository.Verify(r => r.GetAppointmentsByDateAsync(date), Times.Once);
        }
      
    }
}
