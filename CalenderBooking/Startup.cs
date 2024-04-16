using Microsoft.Extensions.DependencyInjection;
using CalenderBooking.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CalenderBooking.Repositories;
using CalenderBooking.Services;

namespace CalenderBooking
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CalenderBookingDbContext>(options =>
            options.UseSqlServer(
            _configuration.GetConnectionString("CalenderBookingConnectionString")));

        // Register other services and repositories
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        }
    }
}