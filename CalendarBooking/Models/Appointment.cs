using CalendarBooking.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace CalendarBooking.Models
{
    public class Appointment : IAppointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public bool IsUniversal { get; set; }  
    }
}
