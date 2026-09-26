using Clinck.Domain.Consts;
using Clinck.Domain.Enums;

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Clinck.Application.ViewModels.PatientVms
{
    public class FormVm
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = Errors.MaxLength), MinLength(3, ErrorMessage = Errors.MinLength)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [MaxLength(20, ErrorMessage = Errors.MaxLength), MinLength(11, ErrorMessage = Errors.MinLength)]
        [RegularExpression(Regex.validPhone, ErrorMessage = Errors.validPhone)]

        [Remote(action: "AllowPhone", controller: "Patient", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = Errors.Unique)]
        public string Phone { get; set; }

        [MaxLength(150, ErrorMessage = Errors.MaxLength)]
        [RegularExpression(Regex.validEmail, ErrorMessage = Errors.validEmail)]
        [Remote(action: "AllowEmail", controller: "Patient", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = Errors.Unique)]
        public string Email { get; set; }

        [Required(ErrorMessage = Errors.Requred)]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = Errors.Requred)]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(250, ErrorMessage = Errors.MaxLength), MinLength(3, ErrorMessage = Errors.MinLength)]
        public string Address { get; set; }

        [MaxLength(1000, ErrorMessage = Errors.MaxLength)]
        public string? MedicalHistory { get; set; }
        
        public IFormFile? Image { get; set; }

        public string? ImageUrl { get; set; }
        public string? ImageThumnail { get; set; }

    }
}
