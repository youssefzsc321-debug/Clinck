using Clinck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Enities
{
    public class Doctor:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
