using System;
using System.Collections.Generic;
using System.Linq;
using CalendarBooking.Data;
using CalendarBooking.Models;
using CalendarBooking.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CalendarBooking.Tests
{
    public class AppointmentRepositoryTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDb")
                .Options;

            return new ApplicationDbContext(options);
        }

        private byte[] GetRowVersion()
        {
            return new byte[] { 0x00 };
        }

        [Fact]
        public void AddAppointment_ShouldAddAppointment()
        {
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context);
            var appointment = new Appointment
            {
                Id = 1,
                DateTime = new DateTime(2024, 5, 26, 9, 0, 0),
                IsUniversal = false,
                RowVersion = GetRowVersion()
            };

            repository.AddAppointment(appointment);
            var fetchedAppointment = repository.GetAppointment(appointment.DateTime);

            Assert.NotNull(fetchedAppointment);
            Assert.Equal(appointment.DateTime, fetchedAppointment.DateTime);
        }

        [Fact]
        public void GetAppointment_ShouldReturnCorrectAppointment()
        {
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context);
            var appointment = new Appointment
            {
                Id = 2,
                DateTime = new DateTime(2024, 5, 26, 9, 0, 0),
                IsUniversal = false,
                RowVersion = GetRowVersion()
            };

            context.Appointments.Add(appointment);
            context.SaveChanges();

            var fetchedAppointment = repository.GetAppointment(appointment.DateTime);

            Assert.NotNull(fetchedAppointment);
            Assert.Equal(appointment.DateTime, fetchedAppointment.DateTime);
        }

        [Fact]
        public void GetAppointments_ShouldReturnAllAppointments()
        {
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context);

            var appointments = new List<Appointment>
            {
                new Appointment { Id = 3, DateTime = new DateTime(2024, 5, 26, 9, 0, 0), IsUniversal = false, RowVersion = GetRowVersion() },
                new Appointment { Id = 4, DateTime = new DateTime(2024, 5, 27, 10, 0, 0), IsUniversal = false, RowVersion = GetRowVersion() }
            };

            context.Appointments.AddRange(appointments);
            context.SaveChanges();

            var fetchedAppointments = repository.GetAppointments().ToList();

            Assert.Equal(2, fetchedAppointments.Count);
        }


        [Fact]
        public void DeleteAppointment_ShouldRemoveAppointment()
        {
            var context = GetInMemoryDbContext();
            var repository = new AppointmentRepository(context);

            var appointment = new Appointment
            {
                Id = 6,
                DateTime = new DateTime(2024, 5, 28, 9, 0, 0),
                IsUniversal = false,
                RowVersion = GetRowVersion()
            };

            repository.AddAppointment(appointment);;
            context.SaveChanges();

            repository.DeleteAppointment(appointment);
            var fetchedAppointment = repository.GetAppointment(appointment.DateTime);

            Assert.Null(fetchedAppointment);
        }
    }
}
