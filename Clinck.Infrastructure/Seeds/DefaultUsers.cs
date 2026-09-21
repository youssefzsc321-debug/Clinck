using Clinck.Domain.Consts;
using Clinck.Domain.Enities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinck.Infrastructure.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedAdminUser(UserManager<AppUser> userManager)
        {
            if(!await userManager.Users.AnyAsync())
            {
                var admin = new AppUser()
                {
                    EmailConfirmed = true,
                    FullName = "Admin",
                    Email="Admin123@gmail.com",
                    UserName= "Admin123@gmail.com"
                };
                var Password = "753951420Tt#";
              var result=await userManager.CreateAsync(admin, Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, AppRoles.Admin);
                }
            }
        }

    }
}
