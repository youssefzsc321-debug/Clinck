using Clinck.Domain.Consts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Infrastructure.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {

                await roleManager.CreateAsync(new IdentityRole() { Name = AppRoles.Admin });
                await roleManager.CreateAsync(new IdentityRole() { Name = AppRoles.Doctor });
                await roleManager.CreateAsync(new IdentityRole() { Name = AppRoles.Receptionist });
                await roleManager.CreateAsync(new IdentityRole() { Name = AppRoles.Patient });
               
            }
        }
    }
}
