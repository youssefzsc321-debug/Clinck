using Clinck.Domain.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.Departments
{
    public class GetDepts
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfDoctors { get; set; }
        public bool IsDeleted { get; set; }
      


        public DateTime CreatedOn { get; set; } = DateTime.Now;
  
        public DateTime? LastUpdatedOn { get; set; }


    }
}
