using Clinck.Domain.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.PatientVms
{
    public class SearchVm
    {
        [Required(ErrorMessage =Errors.Requred)]
        public string Value { get; set; }
    }
}
