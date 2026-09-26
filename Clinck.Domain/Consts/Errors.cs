using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Consts
{
    public static class Errors
    {
        public const string MaxLength = "{0} cannot exceed {1} characters.";
        public const string MinLength = "{0} must be at least {1} characters long.";
        public const string Requred = "{0} is required.";
        public const string Unique = "{0} already exists.";
        public const string validPhone = "Invalid Egyptian phone number.";
        public const string validEmail = "Invalid email format.";
        public const string NotAllowedExtention = "Only .png , .jpg , .jepg Files are allowed!";
        public const string MaxSize = "File Cannot be more than 2MB!";
        public const string appointmentBooked = "This appointment is already booked";
        public const string JustEnglistLetters = "Username must contain only English letters and spaces. Numbers and symbols are not allowed.";
        public const string PasswordNotMatchCritera = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, and one number.";
        public const string PassworNotMatch = "The password and confirmation password do not match.";



    }
}
