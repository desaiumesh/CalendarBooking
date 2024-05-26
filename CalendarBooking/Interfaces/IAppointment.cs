using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarBooking.Interfaces
{
    public interface IAppointment
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsUniversal { get; set; }
    }
}
