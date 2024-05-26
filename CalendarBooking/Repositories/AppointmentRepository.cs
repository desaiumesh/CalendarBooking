using System;
using System.Collections.Generic;
using System.Linq;
using CalendarBooking.Data;
using CalendarBooking.Models;
using CalendarBooking.Interfaces;
using EFCore.BulkExtensions;

namespace CalendarBooking.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Appointment GetAppointment(DateTime dateTime)
        {
            return _context.Appointments.FirstOrDefault(a => a.DateTime == dateTime);
        }

        public IEnumerable<Appointment> GetAppointments()
        {
            return _context.Appointments.ToList();
        }

        public IEnumerable<Appointment> GetUniversalAppointments()
        {
            return _context.Appointments.Where(a => a.IsUniversal).ToList();
        }

        public void AddAppointment(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();
        }

        public void AddAppointments(List<Appointment> appointments)
        {
            _context.BulkInsert(appointments);
            _context.SaveChanges();
        }

        public void UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            _context.SaveChanges();
        }

        public void DeleteAppointment(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
        }
    }
}
