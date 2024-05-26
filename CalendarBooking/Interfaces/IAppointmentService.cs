using System;
using System.Collections.Generic;

namespace CalendarBooking.Interfaces
{
    public interface IAppointmentService
    {
        bool AddAppointment(DateTime dateTime);
        bool DeleteAppointment(DateTime dateTime);
        IEnumerable<DateTime> FindAvailableSlots(DateTime date);
        bool KeepTimeSlot(TimeSpan time);
    }
}
