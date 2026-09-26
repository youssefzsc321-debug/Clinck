using Clinck.Domain.Enities;
using Clinck.Domain.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.Appointment
{
    public class GetApointments
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedOn { get; set; }

        public int PatientId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public AppointmentState AppointmentState { get; set; }  
        public string Doctor { get; set; }
        public string Department { get; set; }

    }
}
