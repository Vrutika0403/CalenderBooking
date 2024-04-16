using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CalenderBooking.Services;
using CalenderBooking.Repositories;
using CalenderBooking.Models;
using CalenderBooking.Database;
using Microsoft.EntityFrameworkCore;

namespace CalenderBooking
{
    class Program
    {
        static async Task Main(string[] args)
        {
           var hostBuilder = new HostBuilder()
            .ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<CalenderBookingDbContext>(options =>
                options.UseSqlServer(
                    "Server=(localdb)\\MSSQLLocalDB;Database=CalenderBooking;Trusted_Connection=True;MultipleActiveResultSets=true"));
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentService, AppointmentService>();
        });

        var host = hostBuilder.Build();
        var appointmentService = host.Services.GetRequiredService<IAppointmentService>();

        await HandleCommandLineInput(appointmentService);
        
        }  
        private static async Task HandleCommandLineInput(IAppointmentService appointmentService)
        {
            while (true)
            {
                Console.Write("Enter command for appointment(ADD, DELETE, FIND, KEEP): ");
                var command = Console.ReadLine();
    
                switch (command?.ToUpperInvariant())
                {
                    case "ADD":
                        Console.Write("Enter date (DD/MM) and time (hh:mm): ");
                        var dateTimeInput = Console.ReadLine();
                        var dateTime = DateTime.ParseExact(dateTimeInput, "dd/MM HH:mm", null);
                        await appointmentService.AddAppointmentAsync(new Appointment { DateTime = dateTime });
                        Console.WriteLine("Appointment added successfully.");
                        break;
                    case "DELETE":
                        Console.Write("Enter date (DD/MM) and time (hh:mm): ");
                        dateTimeInput = Console.ReadLine();
                        dateTime = DateTime.ParseExact(dateTimeInput, "dd/MM HH:mm", null);
                        await appointmentService.DeleteAppointmentAsync(new Appointment { DateTime = dateTime });
                        Console.WriteLine("Appointment deleted successfully.");
                        break;
                    case "FIND":
                        Console.Write("Enter date (DD/MM): ");
                        var dateInput = Console.ReadLine();
                        var date = DateTime.ParseExact(dateInput, "dd/MM", null);
                        var freeTimeSlots = await appointmentService.GetFreeTimeSlotsAsync(date);
                        Console.WriteLine("Free time slots:");
                        foreach (var timeslots in freeTimeSlots)
                        {
                            Console.WriteLine(timeslots.StartTime.ToString("HH:mm"));
                        }
                        break;
                    case "KEEP":
                        Console.Write("Enter time (hh:mm): ");
                        var timeInput = Console.ReadLine();
                        var timeslot = TimeSpan.ParseExact(timeInput, "hh\\:mm", null);
                    
                       // await appointmentService.KeepTimeSlotAsync(timeslot);
                        Console.WriteLine("Time slot kept successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
        }
    }
}

