using CalendarBooking.Interfaces;
using CalendarBooking.Services;
using System;

namespace CalendarBooking.Commands
{
    public static class FindCommand
    {
        public static void Handle(string[] args, IAppointmentService appointmentService)
        {
            if (args == null || args.Length != 1)
            {
                Console.WriteLine("Invalid arguments for FIND command. Format: FIND DD/MM");
                return;
            }

            var datePart = args[0];
            if (!DateTime.TryParseExact(datePart, "dd/MM", null, System.Globalization.DateTimeStyles.None, out var date))
            {
                Console.WriteLine("Invalid date format. Please use format: DD/MM");
                return;
            }


            var slots = appointmentService.FindAvailableSlots(date);

            if (slots.Any())
            {
                Console.WriteLine("Available time slots for " + date.ToString("dd/MM") + ":");
                foreach (var slot in slots)
                {
                    Console.WriteLine(slot.ToString("dd/MM HH:mm"));
                }
            }
            else
            {
                Console.WriteLine("No available time slots for " + date.ToString("dd/MM"));
            }
        }
    }
}
