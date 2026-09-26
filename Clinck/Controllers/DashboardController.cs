using Clinck.Application.Common.Interfaces;
using Clinck.Application.ViewModels.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Clinck.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IApplicationDbContext _context;

        public DashboardController(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new ClinicDashboardVM
            {
                NumberOfPatients = await _context.Patients.CountAsync(),
                NumberOfAppointmentsToday = await _context.Appointments
                    .CountAsync(a => a.AppointmentDate.Date == DateTime.Today),

                LastAddedAppointments = await _context.Appointments
                    .OrderByDescending(a => a.Id)
                    .Take(5)
                    .Select(a => new RecentAppointmentViewModel
                    {
                        Id = a.Id,
                        PatientName = a.Patient.FullName,
                        DoctorName = $"{a.Doctor.FirstName} {a.Doctor.LastName}",
                        DepartmentName = a.Department.Name,
                        Status = a.AppointmentState.ToString(),
                        AppointmentDate = a.AppointmentDate.ToString("yyyy-MM-dd HH:mm")
                    }).ToListAsync(),

                TopDoctors = await _context.Doctors
                    .Select(d => new TopDoctorViewModel
                    {
                        Id = d.Id,
                        FullName = $"{d.FirstName} {d.LastName}",
                        Specialty = d.Department.Name,
                        TotalAppointments = d.Appointments.Count()
                    })
                    .OrderByDescending(d => d.TotalAppointments)
                    .Take(5)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointmentsPerDay(DateTime? startDate, DateTime? endDate)
        {
            startDate ??= DateTime.Today.AddDays(-7);
            endDate ??= DateTime.Today;

            var data = await _context.Appointments
                .Where(a => a.AppointmentDate.Date >= startDate.Value.Date && a.AppointmentDate.Date <= endDate.Value.Date)
                .GroupBy(a => a.AppointmentDate.Date)
                .Select(g => new ChartItemViewModel
                {
                    Label = g.Key.ToString("yyyy-MM-dd"),
                    Value = g.Count()
                })
                .OrderBy(x => x.Label)
                .ToListAsync();

            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPatientsPerDepartment()
        {
            var data = await _context.Departments
                .Select(d => new DepartmentDistributionViewModel
                {
                    DepartmentName = d.Name,
                    Count = d.Doctors.SelectMany(doc => doc.Appointments).Select(a => a.PatientId).Distinct().Count()
                })
                .ToListAsync();

            return Json(data);
        }
    }

}
