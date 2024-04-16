using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CalenderBooking.Models;
using CalenderBooking.Repositories;

namespace CalenderBooking.Tests
{
    public class AppointmentRepositoryTests
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
        public async Task GetAppointmentAsync_ShouldReturnAppointment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CalenderBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CalendarAppDbContext(options))
            {
                var repository = new AppointmentRepository(context);
                var appointment = new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) };
                await context.Appointments.AddAsync(appointment);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAppointmentAsync(appointment.DateTime);

                // Assert
                Assert.Equal(appointment, result);
            }
        }

        [Fact]
        public async Task AddAppointmentAsync_ShouldAddAppointment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CalenderBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CalenderBookingDbContext(options))
            {
                var repository = new AppointmentRepository(context);
                var appointment = new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) };

                // Act
                await repository.AddAppointmentAsync(appointment);

                // Assert
                var result = await context.Appointments.FirstOrDefaultAsync(a => a.DateTime == appointment.DateTime);
                Assert.Equal(appointment, result);
            }
        }

          [Fact]
        public async Task GetAppointmentsByDateAsync_ShouldReturnAppointments()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CalenderBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CalenderBookingDbContext(options))
            {
                var repository = new AppointmentRepository(context);
                var appointments = new List<Appointment>
                {
                    new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) },
                    new Appointment { DateTime = new DateTime(2023, 4, 15, 11, 0, 0) },
                    new Appointment { DateTime = new DateTime(2023, 4, 16, 10, 0, 0) }
                };
                await context.Appointments.AddRangeAsync(appointments);
                await context.SaveChangesAsync();

                // Act
                var result = await repository.GetAppointmentsByDateAsync(new DateTime(2023, 4, 15));

                // Assert
                Assert.Equal(appointments.GetRange(0, 2), result);
            }
        }

          [Fact]
        public async Task DeleteAppointmentAsync_ShouldDeleteAppointment()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CalenderBookingDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new CalenderBookingDbContext(options))
            {
                var repository = new AppointmentRepository(context);
                var appointment = new Appointment { DateTime = new DateTime(2023, 4, 15, 10, 0, 0) };
                await context.Appointments.AddAsync(appointment);
                await context.SaveChangesAsync();

                // Act
                await repository.DeleteAppointmentAsync(appointment);

                // Assert
                var result = await context.Appointments.FirstOrDefaultAsync(a => a.DateTime == appointment.DateTime);
                Assert.Null(result);
            }
        }
