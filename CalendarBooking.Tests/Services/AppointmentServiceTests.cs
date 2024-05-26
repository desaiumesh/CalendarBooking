using System;
using System.Collections.Generic;
using System.Linq;
using CalendarBooking.Interfaces;
using CalendarBooking.Models;
using CalendarBooking.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CalendarBooking.Tests
{
    public class AppointmentServiceTests
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly ILogger<AppointmentService> _logger;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTests()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _logger = new LoggerFactory().CreateLogger<AppointmentService>();
            _appointmentService = new AppointmentService(_appointmentRepositoryMock.Object, _logger);
        }


        [Fact]
        public void AddAppointment_ShouldReturnFalse_ForConflictingTimeSlot()
        {
            // Arrange
            var dateTime = new DateTime(2024, 5, 26, 9, 0, 0);
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointments())
                .Returns(new List<Appointment>
                {
                    new Appointment { DateTime = dateTime }
                });

            // Act
            var result = _appointmentService.AddAppointment(dateTime.AddMinutes(15)); // Conflicting time

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AddAppointment_ShouldReturnTrue_ForValidTimeSlot()
        {
            // Arrange
            var dateTime = new DateTime(2024, 5, 26, 9, 0, 0);
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointments()).Returns(new List<Appointment>());

            // Act
            var result = _appointmentService.AddAppointment(dateTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteAppointment_ShouldReturnFalse_IfAppointmentNotFound()
        {
            // Arrange
            var dateTime = new DateTime(2024, 5, 26, 9, 0, 0);
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointment(dateTime)).Returns((Appointment)null);

            // Act
            var result = _appointmentService.DeleteAppointment(dateTime);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void DeleteAppointment_ShouldReturnTrue_IfAppointmentExists()
        {
            // Arrange
            var dateTime = new DateTime(2024, 5, 26, 9, 0, 0);
            var appointment = new Appointment { DateTime = dateTime };
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointment(dateTime)).Returns(appointment);

            // Act
            var result = _appointmentService.DeleteAppointment(dateTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void FindAvailableSlots_ShouldReturnCorrectSlots()
        {
            // Arrange
            var date = new DateTime(2024, 5, 26);
            var existingAppointments = new List<Appointment>
            {
                new Appointment { DateTime = new DateTime(2024, 5, 26, 9, 0, 0) },
                new Appointment { DateTime = new DateTime(2024, 5, 26, 10, 0, 0) }
            };
            _appointmentRepositoryMock.Setup(repo => repo.GetAppointments()).Returns(existingAppointments);

            // Act
            var slots = _appointmentService.FindAvailableSlots(date).ToList();

            // Assert
            Assert.DoesNotContain(new DateTime(2024, 5, 26, 9, 0, 0), slots);
            Assert.DoesNotContain(new DateTime(2024, 5, 26, 10, 0, 0), slots);
            Assert.Contains(new DateTime(2024, 5, 26, 9, 30, 0), slots);
            Assert.Contains(new DateTime(2024, 5, 26, 10, 30, 0), slots);
        }

        [Fact]
        public void KeepTimeSlot_ShouldReturnFalse_ForInvalidTimeSlot()
        {
            // Arrange
            var time = new TimeSpan(18, 0, 0); // Outside business hours

            // Act
            var result = _appointmentService.KeepTimeSlot(time);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void KeepTimeSlot_ShouldReturnTrue_ForValidTimeSlot()
        {
            // Arrange
            var time = new TimeSpan(10, 0, 0);

            // Act
            var result = _appointmentService.KeepTimeSlot(time);

            // Assert
            Assert.True(result);
        }
    }
}
