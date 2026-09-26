using Clinck.Application.Common.Interfaces;
using Clinck.Domain.Enities;
using Clinck.Infrastructure.Data;
using Clinck.Infrastructure.Seeds;
using Clinck.Web.Services.Contract;
using Clinck.Web.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UoN.ExpressiveAnnotations.NetCore.DependencyInjection;

namespace Clinck
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));


            builder.Services.AddAutoMapper(cfg => {
                cfg.AddProfile<Clinck.Application.Common.Mapping.Doctors.MappingDoctors>();
                cfg.AddProfile<Clinck.Application.Common.Mapping.Department.MappingDepartment>();
                cfg.AddProfile<Clinck.Application.Common.Mapping.Patients.MappingPatients>();
                cfg.AddProfile<Clinck.Application.Common.Mapping.Appointment.MappingAppointment>();
                cfg.AddProfile<Clinck.Application.Common.Mapping.User>();
            });

            builder.Services.AddScoped<IDocotorService, DocotorService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddExpressiveAnnotations();

            // Identity Configuration with AppUser
            builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.

                options.Password.RequireNonAlphanumeric = false;

                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";

            });

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(8);
                options.Lockout.MaxFailedAccessAttempts = 4;
                options.Lockout.AllowedForNewUsers = true;
            });

            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            // Seed Data
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            await DefaultRoles.SeedRoles(roleManager);
            await DefaultUsers.SeedAdminUser(userManager);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}