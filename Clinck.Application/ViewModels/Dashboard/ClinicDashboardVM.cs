using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Application.ViewModels.Dashboard
{
    public class ClinicDashboardVM
    {
        public int NumberOfPatients { get; set; }
        public int NumberOfAppointmentsToday { get; set; }

        public IEnumerable<RecentAppointmentViewModel> LastAddedAppointments { get; set; } = new List<RecentAppointmentViewModel>();

        public IEnumerable<TopDoctorViewModel> TopDoctors { get; set; } = new List<TopDoctorViewModel>();
    }
    public class RecentAppointmentViewModel
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string DepartmentName { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public string Status { get; set; } 
        public string AppointmentDate { get; set; }
    }
    public class TopDoctorViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Specialty { get; set; }
        public string ImageThumbnailUrl { get; set; }
        public int TotalAppointments { get; set; }
    }
    public class ChartItemViewModel
    {
        public string Label { get; set; } 
        public decimal Value { get; set; } 
    }

    public class DepartmentDistributionViewModel
    {
        public string DepartmentName { get; set; } 
        public int Count { get; set; } 
    }
}
