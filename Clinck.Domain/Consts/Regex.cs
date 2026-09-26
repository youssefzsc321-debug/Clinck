using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Consts
{
    public static class Regex
    {
        public const string validPhone = @"^01[0125][0-9]{8}$";
        public const string validEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        public const string PasswordPattern = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d\\W_]{8,}$";
        public const string UserNamePattern = @"^[a-zA-Z0-9-._@+#]+$";
        public const string AllowJustEnglish = @"^[a-zA-Z\s]+$";
    }
}
