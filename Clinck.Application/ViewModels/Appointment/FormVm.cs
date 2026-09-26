using Clinck.Domain.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinck.Application.ViewModels.Appointment
{
    public class FormVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.Requred)]
        [Remote(action: "ValidAppointment", controller: "Appointment", AdditionalFields = "SelectedDoctor,Id", HttpMethod = "POST", ErrorMessage = Errors.appointmentBooked)]
        public DateTime AppointmentDate { get; set; }

        public int PatientId { get; set; } 

        [Required(ErrorMessage = Errors.Requred)]
        public int SelectedDoctor { get; set; }
        public IEnumerable<SelectListItem>? Doctors { get; set; } = new List<SelectListItem>();

        [Required(ErrorMessage = Errors.Requred)]
        public int SelectedDepartment { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; } = new List<SelectListItem>();
    }
}