using Clinck.Domain.Enities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Clinck.Application.ViewModels.Doctors
{
    public class FormVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Doctor Name is required.")]
        [Display(Name = "First Name")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]

        public string FirstName { get; set; }
        //public string Name { get; set; }
        [Required(ErrorMessage = "Doctor Name is required.")]
        [Display(Name = "Second Name")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please select a department.")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    }
}
