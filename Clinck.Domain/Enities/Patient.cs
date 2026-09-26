using Clinck.Domain.Common;
using Clinck.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Clinck.Domain.Enities
{
    [Index(nameof(Phone), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    [Authorize]
    public class Patient:BaseEntity
    {
        [MaxLength(100),MinLength(3)]
        public string FullName { get; set; }

        [MaxLength(20),MinLength(11)]
        public string Phone { get; set; }
        [MaxLength(150)]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        [MaxLength(250),MinLength(3)]
        public string Address { get; set; }

        [MaxLength(1000)]
        public string? MedicalHistory { get; set; }
        public bool IsDeleted { get; set; }

        public string Image { get; set; }
        public string ImageThumnail { get; set; }

        public ICollection<Appointment> Appointments { get; set; }


    }
}
