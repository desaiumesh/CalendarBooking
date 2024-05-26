using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CalendarBooking.Commands;
using CalendarBooking.DependencyInjection;
using CalendarBooking.Services;
using System;
using System.Globalization;
using CalendarBooking.Interfaces;
using CalendarBooking.Data;
using CalendarBooking.Repositories;

class Program
{
    static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddLogging(configure => configure.AddConsole())
            .AddDbContext<ApplicationDbContext>()
            .AddScoped<IAppointmentRepository, AppointmentRepository>()
            .AddScoped<IAppointmentService, AppointmentService>()
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<ILogger<Program>>();
        var appointmentService = serviceProvider.GetService<IAppointmentService>();

        try
        {
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine("Enter command:");
                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid command. Please try again.");
                    continue;
                }

                var commandParts = input.Split(' ');
                var command = commandParts[0].ToUpper();
                var parameters = commandParts.Skip(1).ToArray();

                switch (command)
                {
                    case "ADD":
                        AddCommand.Handle(parameters, appointmentService);
                        break;
                    case "DELETE":
                        DeleteCommand.Handle(parameters, appointmentService);
                        break;
                    case "FIND":
                        FindCommand.Handle(parameters, appointmentService);
                        break;
                    case "KEEP":
                        KeepCommand.Handle(parameters, appointmentService);
                        break;
                    case "EXIT":
                        return;
                    default:
                        Console.WriteLine("Invalid command. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while executing the command.");
        }

    }
}
