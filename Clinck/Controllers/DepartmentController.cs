using AutoMapper;
using Clinck.Application.ViewModels.Departments;
using Clinck.Domain.Consts;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinck.Web.Controllers
{
    [Authorize(Roles =AppRoles.Admin)]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _deptService;
        private readonly IMapper mapper;
        public DepartmentController(IDepartmentService deptService, IMapper mapper)
        {
            _deptService = deptService;
            this.mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var depts =await _deptService.GetAllWithDoctorsAsync();
            var modelVm=mapper.Map<IEnumerable<GetDepts>>(depts);
            return View(modelVm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var modelVm = new FormVm();
            return PartialView("_Form", modelVm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FormVm model)
        {
            if(!ModelState.IsValid)
            {
                return PartialView("_Form", model);
            }
            var dept = mapper.Map<Department>(model);
            dept.CreatedOn = DateTime.Now;
            dept.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _deptService.Add(dept);
            await _deptService.SaveChanges();

            var savedDept=await _deptService.GetDeptWithDoctorsById(dept.Id);
            var deptVm=mapper.Map<GetDepts>(savedDept);
            return PartialView("_DeptRow", deptVm);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var dept=await _deptService.GetDeptById(id);
            if (dept == null) return NotFound();
            var deptVm=mapper.Map<FormVm>(dept);
            return PartialView("_Form", deptVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FormVm model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_Form", model);
            }
            var dept =await _deptService.GetDeptById(model.Id);
            if(dept == null) return NotFound();
            dept = mapper.Map(model, dept);
            dept.LastUpdatedOn= DateTime.Now;
            dept.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _deptService.SaveChanges();
            var updatedDept=await _deptService.GetDeptWithDoctorsById(dept.Id);
            var deptVm = mapper.Map<GetDepts>(updatedDept);
            return PartialView("_DeptRow", deptVm);
        }
        [HttpPost]
        public async Task<IActionResult> ToggleState(int id)
        {
            var dept =await _deptService.GetDeptById(id);
            if (dept == null) return NotFound();
            dept.IsDeleted=!dept.IsDeleted;
            dept.LastUpdatedOn = DateTime.Now;
            dept.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _deptService.SaveChanges();
            return Ok(dept.LastUpdatedOn.ToString());

        }

        public async Task<IActionResult> AllowName(FormVm model)
        {
            var dept=await _deptService.GetByName(model.Name);
            var isValid = dept is null || dept.Id == model.Id;
            return Json(isValid);
        }


    }
}
