using AutoMapper;
using Clinck.Application.ViewModels.Appointment;
using Clinck.Domain.Enities;
using Clinck.Domain.Enums;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinck.Web.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IDepartmentService _departmentService;
        private readonly IDocotorService _docotorService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;
        public AppointmentController(IPatientService patientService, IMapper _mapper, IDepartmentService departmentService, IDocotorService docotorService, IAppointmentService appointmentService)
        {
            _patientService = patientService;
            this._mapper = _mapper;
            _departmentService = departmentService;
            _docotorService = docotorService;
            _appointmentService = appointmentService;
        }
        public async Task<IActionResult> Index(int patientId)
        {
            var pat = await _patientService.GetByIdWithAppointment(patientId);
            if (pat == null) return NotFound();
            var appintments = pat.Appointments;
            var isUpdated = false;
            foreach(var app in appintments)
            {
                if (app.AppointmentDate < DateTime.Now.AddHours(1)) 
                {
                    app.AppointmentState = AppointmentState.Cancelled;
                    isUpdated = true;
                }
            }
            if (isUpdated)await _appointmentService.SaveChanges();
            var modelVm = _mapper.Map<IEnumerable<GetApointments>>(appintments); 
            return View(modelVm);
          
        }

        [HttpGet]
        public async Task<IActionResult> Create(int patientId)
        {
            var pat = await _patientService.GetById(patientId);
            if (pat == null) return NotFound();
            var modelVm = new FormVm()
            {
                PatientId = patientId,
            };
            modelVm = await FillModel(modelVm);
            return View("Form", modelVm);

        }

        [HttpPost]
        public async Task<IActionResult> Create(FormVm model)
        {
            if (!ModelState.IsValid)
            {
                model = await FillModel(model);
                return View("Form", model);
            }
            var app = new Appointment()
            {
                DoctorId = model.SelectedDoctor,
                DepartmentId = model.SelectedDepartment,
                AppointmentDate = model.AppointmentDate,
                PatientId = model.PatientId,
            };
            app.CreatedOn = DateTime.Now;
            app.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await _appointmentService.Add(app);
            await _appointmentService.SaveChanges();

            return RedirectToAction(actionName: "Details", controllerName: "Patient", new { id = app.PatientId });


        }



        private async Task<FormVm> FillModel(FormVm? model = null)
        {
            var modelVm = model ?? new FormVm();
            var allDepts = await _departmentService.GetAllAsync();
            modelVm.Departments = allDepts.Select(d => new SelectListItem { Text = d.Name, Value = d.Id.ToString() });
            if (modelVm.SelectedDepartment != 0)
            {
                var dept = await _departmentService.GetDeptWithDoctorsById(modelVm.SelectedDepartment);
                modelVm.Doctors = dept.Doctors.Select(d => new SelectListItem { Text = $"{d.FirstName} {d.LastName}", Value = d.Id.ToString() });
            }
            else
            {
                modelVm.Doctors = new List<SelectListItem>();
            }
            return modelVm;

        }

        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            var dept = await _departmentService.GetDeptWithDoctorsById(departmentId);
            var doctors = dept.Doctors.Select(d => new SelectListItem { Text = $"{d.FirstName} {d.LastName}", Value = d.Id.ToString() });
            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> ValidAppointment(FormVm model)
        {
            var app = await _appointmentService.GetAppointmentWithDoctor(model.AppointmentDate, model.SelectedDoctor);
            var isValid = app is null || model.Id == app.Id;
            return Json(isValid);
        }

        public async Task<IActionResult> ChangeState(int id,bool cancel=true)
        {
            var app = await _appointmentService.GetAppById(id);
            if (app is null) return NotFound();
            app.LastUpdatedOn= DateTime.Now;
            app.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            app.AppointmentState = (cancel?AppointmentState.Cancelled:AppointmentState.Completed);
            await _appointmentService.SaveChanges();
            return Ok(app.LastUpdatedOn.ToString());
        }
        


    }
}
