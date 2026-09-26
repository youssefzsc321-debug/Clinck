using AutoMapper;
using Clinck.Application.Common.Mapping;
using Clinck.Application.ViewModels.UserVms;
using Clinck.Domain.Consts;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinck.Web.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UserController : Controller
    {
        private readonly IUserService _user;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(IUserService user, IMapper _mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _user = user;
            this._mapper = _mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _user.GetAllAsync();
            var modelVm = _mapper.Map<IEnumerable<UserVm>>(users);
            return View(modelVm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UserForm()
            {
                Roles = await roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync()
            };
            return View("Form", model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserForm model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync();
                return View(model);
            }
            var newUser = new AppUser()
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                CreatedOn = DateTime.Now,
                CreatedById = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
            };
            try
            {
                await _user.Add(newUser, model);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                model.Roles = await roleManager.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToListAsync();
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _user.GetById(id);
            if (user is null) return NotFound();
            var model = _mapper.Map<UserForm>(user);
            model.SelectedRoles = await userManager.GetRolesAsync(user);
            model.Roles = await roleManager.Roles.Select(r => new SelectListItem() { Text = r.Name, Value = r.Name }).ToListAsync();
            return View("Form", model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserForm model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = await roleManager.Roles
            .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
            .ToListAsync();
                return View("Form", model);
            }
            var user =await _user.GetById(model.Id);
            if (user is null) return NotFound();
            user = _mapper.Map(model, user);
            user.LastUpdatedById = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            user.LastUpdatedOn = DateTime.Now;
            var res = await userManager.UpdateAsync(user);
            if (res.Succeeded)
            {

                var currnetRoles = await userManager.GetRolesAsync(user);
                var isSame = currnetRoles.SequenceEqual(model.SelectedRoles);
                if (!isSame)
                {
                    await userManager.RemoveFromRolesAsync(user, currnetRoles);
                    await userManager.AddToRolesAsync(user, model.SelectedRoles);
                }
                await userManager.UpdateSecurityStampAsync(user);
                var modelVm = _mapper.Map<UserVm>(user);
                return RedirectToAction(nameof(Index));


            }

            return BadRequest(string.Join(",", res.Errors.Select(e => e.Description)));
        }

        public async Task<IActionResult> ToggleStatus(string id)
        {
            var user = await _user.GetById(id);
            if (user is null) return NotFound();
            user.IsDeleted = !user.IsDeleted;
            user.LastUpdatedOn = DateTime.Now;
            user.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await userManager.UpdateAsync(user);


            if (!result.Succeeded)
            {
                return BadRequest(string.Join(",", result.Errors.Select(e => e.Description)));
            }
            if (user.IsDeleted)
            {
                await userManager.UpdateSecurityStampAsync(user);
            }
            return Ok(user.LastUpdatedOn.ToString());

        }
        public async Task<IActionResult> AllowUserName(UserForm model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            var allow = user is null || model.Id == user.Id;
            return Json(allow);
        }
        public async Task<IActionResult> AllowUserEmail(UserForm model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var allow = user is null || model.Id == user.Id;
            return Json(allow);
        }

    }



}

