using AspNetCoreGeneratedDocument;
using AutoMapper;
using Clinck.Application.ViewModels.Doctors;
using Clinck.Domain.Consts;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Security.Claims;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Clinck.Web.Controllers
{
    [Authorize(Roles =AppRoles.Admin)]
    public class DoctorsController : Controller
    {
        private readonly IDocotorService _service;
        private readonly IMapper mapper;
        private readonly IDepartmentService departmentService;
        public DoctorsController(IDocotorService service, IMapper mapper, IDepartmentService departmentService)
        {
            _service = service;
            this.mapper = mapper;
            this.departmentService = departmentService;
        }
        public async Task<IActionResult> Index()
        {
            var docotors = await _service.GetAllAsync();
            var modelVm = mapper.Map<IEnumerable<GetDoctorsVm>>(docotors);
            return View(modelVm);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var modelVm = new FormVm();
            var departments = await departmentService.GetAllAsync();
            modelVm.Departments = departments.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name });
            return PartialView("_Form", modelVm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FormVm model)
        {
            if (!ModelState.IsValid)
            {
                var modelVm = new FormVm();
                var departments = await departmentService.GetAllAsync();
                modelVm.Departments = departments.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name });
                return PartialView("_Form", modelVm);
            }
            var doctor = mapper.Map<Doctor>(model);
            doctor.CreatedOn = DateTime.Now;
            doctor.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _service.Add(doctor);
            await _service.SaveChanges();
            var savedDoctor = await _service.GetByIdWithDeptAsync(doctor.Id);

            var doctorVm = mapper.Map<GetDoctorsVm>(savedDoctor);
            return PartialView("_DoctorRow", doctorVm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var doc = await _service.GetByIdAsync(id);
            if (doc == null) return BadRequest();
            var modelVm = mapper.Map<FormVm>(doc);
            var depts = await departmentService.GetAllAsync();
            modelVm.Departments = depts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name });
            modelVm.DepartmentId = doc.DepartmentId;
            return PartialView("_Form", modelVm);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(FormVm model)
        {
            if (!ModelState.IsValid)
            {
                var depts = await departmentService.GetAllAsync();
                model.Departments = depts.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name });
                return PartialView("_Form", model);
            }
            var doc = await _service.GetByIdAsync(model.Id);
            if (doc == null) return BadRequest();
            doc = mapper.Map(model, doc);
            doc.LastUpdatedOn = DateTime.Now;
            doc.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _service.SaveChanges();
            var savedDoctor=await _service.GetByIdWithDeptAsync(model.Id);
            var docVm = mapper.Map<GetDoctorsVm>(savedDoctor);
            return PartialView("_DoctorRow", docVm);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleState(int id)
        {
            var doc = await _service.GetByIdAsync(id);
            if (doc == null) return BadRequest();
            doc.LastUpdatedOn = DateTime.Now;
            doc.IsDeleted = !doc.IsDeleted;
            doc.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _service.SaveChanges();
            return Ok(doc.LastUpdatedOn.ToString());
        }

        






    }
}
