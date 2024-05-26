using CalendarBooking.Interfaces;
using CalendarBooking.Services;
using System;
using System.Globalization;

namespace CalendarBooking.Commands
{
    public static class DeleteCommand
    {
        public static void Handle(string[] args, IAppointmentService appointmentService)
        {
            if (args == null || args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine("Invalid arguments for DELETE command. Format: DELETE DD/MM hh:mm or DELETE DD/MM hh:mm tt");
                return;
            }

            var datePart = args[0];
            var timePart = args[1];
            var timeSpecifier = args.Length == 3 ? args[2] : null;

            if (!DateTime.TryParseExact(datePart + " " + timePart + (timeSpecifier != null ? " " + timeSpecifier : ""), new[] { "dd/MM HH:mm", "dd/MM h:mm tt" }, null, System.Globalization.DateTimeStyles.None, out var dateTime))
            {
                Console.WriteLine("Invalid date/time format. Please use format: DD/MM hh:mm or DD/MM h:mm tt");
                return;
            }

            var result = appointmentService.DeleteAppointment(dateTime);
            Console.WriteLine(result ? "Appointment deleted successfully." : "Failed to delete appointment.");
        }
    }
}
