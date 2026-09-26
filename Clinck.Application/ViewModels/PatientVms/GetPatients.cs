using Clinck.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.PatientVms
{
    public class GetPatients
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public string MedicalHistory { get; set; }
        public bool IsDeleted { get; set; }
        public string ImageThumnail { get; set; }
    }
}
