using Clinck.Domain.Consts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Clinck.Application.ViewModels.UserVms
{
    public class UserForm
    {
        public string? Id { get; set; }
        [MaxLength(100, ErrorMessage = Errors.MaxLength)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [MaxLength(50, ErrorMessage = Errors.MaxLength)]
        [Display(Name = "User Name")]
        [Remote(action: "AllowUserName", controller: "User", AdditionalFields = "Id", ErrorMessage = Errors.Unique)]
        [RegularExpression(Regex.AllowJustEnglish, ErrorMessage = Errors.JustEnglistLetters)]
        public string UserName { get; set; }
        [MaxLength(150, ErrorMessage = Errors.MaxLength)]
        [EmailAddress]
        [Remote(action: "AllowUserEmail", controller: "User", AdditionalFields = "Id", ErrorMessage = Errors.Unique)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [RegularExpression(Regex.PasswordPattern, ErrorMessage = Errors.PasswordNotMatchCritera)]
        [RequiredIf("Id==null", ErrorMessage = Errors.Requred)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = Errors.PassworNotMatch)]
        [RequiredIf("Id==null", ErrorMessage = Errors.Requred)]
        public string? ConfirmPassword { get; set; }
        public IList<string> SelectedRoles { get; set; } = new List<string>();
        public IEnumerable<SelectListItem>? Roles { get; set; }=new List<SelectListItem>();

    }
}
