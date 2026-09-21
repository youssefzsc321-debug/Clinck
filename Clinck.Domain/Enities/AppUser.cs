using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Enities
{
    public class AppUser: IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? LastUpdatedOn { get; set; }

        public string? CreatedById { get; set; }
        public string? LastUpdatedById { get; set; }
    }
}
