using Clinck.Domain.Common;
using Clinck.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Enities
{
    public class Appointment:BaseEntity
    {
        public DateTime AppointmentDate { get; set; }
        public AppointmentState AppointmentState { get; set; } = AppointmentState.Scheduled;

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }


    }
}
