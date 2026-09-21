using Clinck.Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Domain.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }=DateTime.Now;
        public string CreatedById { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedById { get; set; }

        public AppUser CreatedBy { get; set; }
        public AppUser LastUpdatedBy { get; set; }


    }
}
