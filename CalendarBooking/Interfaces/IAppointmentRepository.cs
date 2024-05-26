
 using System;
 using System.Collections.Generic;
 using CalendarBooking.Models;

namespace CalendarBooking.Interfaces
{
    public interface IAppointmentRepository
    {
        Appointment GetAppointment(DateTime dateTime);
        IEnumerable<Appointment> GetAppointments();
        IEnumerable<Appointment> GetUniversalAppointments();
        void AddAppointment(Appointment appointment);
        void AddAppointments(List<Appointment> appointments);
        void UpdateAppointment(Appointment appointment);
        void DeleteAppointment(Appointment appointment);
    }
}
