using CalendarBooking.Interfaces;
using CalendarBooking.Models;
using CalendarBooking.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarBooking.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository appointmentRepository, ILogger<AppointmentService> logger)
        {
            _appointmentRepository = appointmentRepository;
            _logger = logger;
        }

        public bool AddAppointment(DateTime dateTime)
        {
            try
            {
                if (!IsValidTimeSlot(dateTime))
                {
                    _logger.LogWarning("Invalid time slot.");
                    return false;
                }

                if (IsTimeSlotConflicting(dateTime))
                {
                    _logger.LogWarning("Time slot conflicts with an existing appointment.");
                    return false;
                }

                var appointment = new Appointment { DateTime = dateTime, IsUniversal = false };
                _appointmentRepository.AddAppointment(appointment);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding appointment.");
                return false;
            }
        }

        public bool DeleteAppointment(DateTime dateTime)
        {
            try
            {
                var appointment = _appointmentRepository.GetAppointment(dateTime);
                if (appointment == null)
                {
                    _logger.LogWarning("Appointment not found.");
                    return false;
                }

                _appointmentRepository.DeleteAppointment(appointment);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning("Concurrency conflict detected while deleting appointment.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting appointment.");
                return false;
            }
        }

        public IEnumerable<DateTime> FindAvailableSlots(DateTime date)
        {
            var slots = new List<DateTime>();
            var startTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            var endTime = new DateTime(date.Year, date.Month, date.Day, 17, 00, 0);

            var existingAppointments = _appointmentRepository.GetAppointments()
             .Where(a => a.DateTime.Date == date.Date)
             .OrderBy(a => a.DateTime)
             .ToList();

            while (startTime <= endTime)
            {
                // Find the next available 30-minute slot
                bool slotAvailable = true;
                foreach (var appointment in existingAppointments)
                {
                    var appointmentEnd = appointment.DateTime.AddMinutes(30);
                    if ((startTime >= appointment.DateTime && startTime < appointmentEnd) ||
                        (startTime.AddMinutes(30) > appointment.DateTime && startTime < appointmentEnd))
                    {
                        slotAvailable = false;
                        startTime = appointmentEnd;  
                        break;
                    }
                }

                if (slotAvailable)
                {
                    if(!IsSecondDayOfThirdWeek(startTime))
                      slots.Add(startTime);

                    startTime = startTime.AddMinutes(30);  
                }
            }

            return slots;
        }

        public bool KeepTimeSlot(TimeSpan time)
        {
            try
            {
                if (!IsValidTimeSlotForKeeping(time))
                {
                    _logger.LogWarning("Invalid time slot.");
                    return false;
                }

                var today = DateTime.Today;
                var endOfYear = new DateTime(today.Year, 12, 31);
                var appointments = new List<Appointment>();

                for (var date = today; date <= endOfYear; date = date.AddDays(1))
                {
                    var appointmentDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0);

                    // Skip the second day of the third week of any month
                    if (IsSecondDayOfThirdWeek(appointmentDateTime))
                        continue;

                    appointments.Add(new Appointment
                    {
                        DateTime = appointmentDateTime,
                        IsUniversal = true
                    });
                }

                _appointmentRepository.AddAppointments(appointments);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error keeping time slot.");
                return false;
            }
        }

        private bool IsValidTimeSlot(DateTime dateTime)
        {
            // Business hours check
            if (dateTime.Hour < 9 || (dateTime.Hour >= 17 && dateTime.Minute > 0))
                return false;

            // Check if time is universally kept
            if (_appointmentRepository.GetUniversalAppointments().Any(a => a.DateTime.TimeOfDay == dateTime.TimeOfDay))
                return false;

            if (IsSecondDayOfThirdWeek(dateTime))
            {
                _logger.LogWarning("Time slot is reserved and unavailable (2nd day of third week)");
                return false;
            }

            return true;
        }

        private bool IsValidTimeSlotForKeeping(TimeSpan time)
        {
            // Business hours check
            if (time < new TimeSpan(9, 0, 0) || time >= new TimeSpan(17, 0, 0))
                return false;

            return true;
        }

        private bool IsTimeSlotConflicting(DateTime dateTime)
        {
            // Check for overlapping appointments within 30 minutes
            var existingAppointments = _appointmentRepository.GetAppointments();
            return existingAppointments.Any(a => Math.Abs((a.DateTime - dateTime).TotalMinutes) < 30);
        }

        private bool IsSecondDayOfThirdWeek(DateTime dateTime)
        {
            var firstDayOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1);

            DateTime thirdWeekSecondDay;
            if (firstDayOfMonth.DayOfWeek == DayOfWeek.Monday)
            {
                thirdWeekSecondDay = firstDayOfMonth.AddDays(14 + 1); 
            }
            else
            {
                var daysUntilMonday = ((int)DayOfWeek.Monday - (int)firstDayOfMonth.DayOfWeek + 7) % 7;
                var firstMonday = firstDayOfMonth.AddDays(daysUntilMonday);
                thirdWeekSecondDay = firstMonday.AddDays(14 + 1); 
            }

            return dateTime.Date == thirdWeekSecondDay.Date &&  dateTime.Hour == 16;
        }
    }
}
