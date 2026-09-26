using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.UserVms
{
    public class UserVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
    }
}
