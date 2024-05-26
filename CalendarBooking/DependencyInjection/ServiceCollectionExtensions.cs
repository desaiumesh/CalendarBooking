using Microsoft.Extensions.DependencyInjection;
using CalendarBooking.Data;
using CalendarBooking.Services;
using Microsoft.EntityFrameworkCore;
using CalendarBooking.Interfaces;

namespace CalendarBooking.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            return services;
        }
    }
}
