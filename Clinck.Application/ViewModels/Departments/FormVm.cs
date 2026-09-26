using Clinck.Domain.Consts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Clinck.Application.ViewModels.Departments
{
    public class FormVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = Errors.Requred)]
        [MaxLength(100, ErrorMessage =Errors.MaxLength )]
        [MinLength(3, ErrorMessage =Errors.MinLength )]
        [Remote(action: "AllowName", controller: "Department", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage =Errors.Unique )]
        public string Name { get; set; }
    }
}
