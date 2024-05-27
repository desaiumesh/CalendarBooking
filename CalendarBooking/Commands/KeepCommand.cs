using CalendarBooking.Interfaces;
using CalendarBooking.Services;
using System;
using System.Globalization;

namespace CalendarBooking.Commands
{
    public static class KeepCommand
    {
        public static void Handle(string[] args, IAppointmentService appointmentService)
        {
            if (args == null || args.Length < 1 || args.Length > 2)
            {
                Console.WriteLine("Invalid arguments for KEEP command. Format: KEEP hh:mm or KEEP hh:mm tt");
                return;
            }

            var timePart = args[0];
            var timeSpecifier = args.Length == 2 ? args[1] : (args.Length == 3 ? args[2] : null);

            TimeSpan time;

            if (timeSpecifier == null)
            {
                // Try parsing as 24-hour format
                if (!TimeSpan.TryParseExact(timePart, "hh\\:mm", CultureInfo.InvariantCulture, out time))
                {
                    Console.WriteLine("Invalid time format. Please use format: hh:mm or hh:mm tt");
                    return;
                }
            }
            else
            {
                // Try parsing as 12-hour format
                var timeString = timePart + " " + timeSpecifier;
                if (!DateTime.TryParseExact(timeString, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    Console.WriteLine("Invalid time format. Please use format: hh:mm or hh:mm tt");
                    return;
                }
                time = dateTime.TimeOfDay;
            }


            var result = appointmentService.KeepTimeSlot(time);
            Console.WriteLine(result ? "Time slot kept successfully." : "Failed to keep time slot.");
        }
    }
}
