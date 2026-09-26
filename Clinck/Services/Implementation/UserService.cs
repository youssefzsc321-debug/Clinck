using AutoMapper;
using Azure.Core;
using Clinck.Application.Common.Interfaces;
using Clinck.Application.ViewModels.UserVms;
using Clinck.Domain.Enities;
using Clinck.Web.Services.Contract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Text.Encodings.Web;

namespace Clinck.Web.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IApplicationDbContext _context;
        public UserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context= context;
        }
        public async Task Add(AppUser user, UserForm modelVm)
        {
            var res = await userManager.CreateAsync(user, modelVm.Password);
            if (res.Succeeded)
            {
                if (modelVm.SelectedRoles != null && modelVm.SelectedRoles.Any())
                {
                    await userManager.AddToRolesAsync(user, modelVm.SelectedRoles);
                }
            }
            else
            {
                throw new Exception(string.Join(", ", res.Errors.Select(e => e.Description)));
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await userManager.Users.ToListAsync();
        }

        public async Task<AppUser?> GetById(string id)
        {
            return await userManager.FindByIdAsync(id);
        }

        public async Task<AppUser?> GetByName(string name)
        {
            return await userManager.FindByNameAsync(name);
        }

        public async Task<int> SaveChanges()
        {
           return await _context.SaveChangesAsync();
        }
    }
}
