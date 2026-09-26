using AutoMapper;
using Clinck.Application.ViewModels.PatientVms;
using Clinck.Domain.Consts;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinck.Web.Controllers
{
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Doctor}")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public PatientController(IPatientService patientService, IMapper mapper, IImageService imageService)
        {
            _patientService = patientService;
            _mapper = mapper;
            _imageService = imageService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SearchForPatient(SearchVm model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var patient=await _patientService.GetByPhone(model.Value) ??await _patientService.GetByEmail(model.Value);
            if (patient == null) return PartialView("_DrawPatient", null);
            var modelVm = new DrawPatientVm()
            {
                Id = patient.Id,
                Name = patient.FullName,
                ImageThumnail = patient.ImageThumnail,
            };
            return PartialView("_DrawPatient", modelVm);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var model = new FormVm();
            return View("Form",model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FormVm model)
        {
            if (model.Id == 0 && (model.Image == null || model.Image.Length == 0))
            {
                ModelState.AddModelError("Image", "The Patient Picture field is required.");
            }
            if (!ModelState.IsValid) return View("Form",model);
            var patient=_mapper.Map<Patient>(model);    
            if(model.Image!=null)
            {
                var extention = Path.GetExtension(model.Image.FileName);
                var imageName = $"{Guid.NewGuid()}{extention}";
                var folderName = "Images/Patients";
                var (isUploaded, errorMessage) = await _imageService.UploadAsync(
                    model.Image,
                     imageName,
                    folderName,
                    true
                );
                if (isUploaded)
                {
                    patient.Image = $"/{folderName}/{imageName}";
                    patient.ImageThumnail = $"/{folderName}/Thumb/{imageName}";
                }
                else
                {
                    return BadRequest(errorMessage);
                }
            }
            else
            {
                ModelState.AddModelError("Image", Errors.Requred);
                return View("Form", model);
            }
            patient.CreatedOn=DateTime.Now;
            patient.CreatedById= User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _patientService.Add(patient);
            await _patientService.SaveChanges();
            var savedPat=await _patientService.GetById(patient.Id);
            return RedirectToAction(nameof(Details), new { id = savedPat.Id });


        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var pat=await _patientService.GetById(id);
            if (pat == null) return NotFound();
            var modelVm = _mapper.Map<GetPatients>(pat);
            return View(modelVm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var pat = await _patientService.GetById(id);
            if(pat == null) return NotFound();
            var modelVm = _mapper.Map<FormVm>(pat);
            return View("Form",modelVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FormVm model)
        {
            if (!ModelState.IsValid) return View("Form", model);
            var pat = await _patientService.GetById(model.Id);
            if(pat==null) return NotFound();
            if(model.Image is not null)
            {
                if(pat.Image is not null)
                {
                    _imageService.Delete(pat.Image,pat.ImageThumnail);
                }
                var extention = Path.GetExtension(model.Image.FileName);
                var imageName = $"{Guid.NewGuid()}{extention}";
                var folderName = "Images/Patients";
                var (isUploaded, errorMessage) = await _imageService.UploadAsync(
                    model.Image,
                     imageName,
                    folderName,
                    true
                );
                if (isUploaded)
                {
                    model.ImageUrl = $"/{folderName}/{imageName}";
                    model.ImageThumnail = $"/{folderName}/Thumb/{imageName}";
                }
                else
                {
                    return BadRequest(errorMessage);
                }

            }
            else if(model.Image is null &&pat.Image is not null)
            {
                model.ImageUrl = pat.Image;
                model.ImageThumnail=pat.ImageThumnail;
            }
            pat=_mapper.Map(model, pat);
            pat.LastUpdatedOn= DateTime.Now;
            pat.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _patientService.SaveChanges();
            return RedirectToAction(nameof(Details), new {id=pat.Id});
        }
        [HttpPost]
        public async Task<IActionResult> AllowPhone(FormVm model)
        {
            var pat=await _patientService.GetByPhone(model.Phone);
            var isValid = pat is null || pat.Id == model.Id;
            return Json(isValid);
        }
        [HttpPost]
        public async Task<IActionResult> AllowEmail(FormVm model)
        {
            var pat=await _patientService.GetByEmail(model.Email);
            var isValid = pat is null || pat.Id == model.Id;
            return Json(isValid);
        }

    }
}
