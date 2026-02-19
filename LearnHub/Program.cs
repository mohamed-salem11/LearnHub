
using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using LearnHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearnHub
{
     
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

         
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

            builder.Services.AddScoped<IEnrollmentRepository, EfEnrollmentRepository>();
            builder.Services.AddScoped<EnrollmentService>();

            builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
            builder.Services.AddScoped<CategoryService>();

            builder.Services.AddScoped<ICourseRepository, EfCourseRepository>();
            builder.Services.AddScoped<CourseService>();

            builder.Services.AddScoped<ILessonRepository, EfLessonRepository>();
            builder.Services.AddScoped<LessonService>();

            builder.Services.AddScoped<IAdminRepository, EfAdminRepository>();
            builder.Services.AddScoped<AdminService>();

            builder.Services.AddScoped<IInstructorRepository, EfInstructorRepository>();
            builder.Services.AddScoped<InstructorService>();

            builder.Services.AddScoped<IInstructorRequestRepository, EfInstructorRequestRepository>();
            builder.Services.AddScoped<InstructorRequestService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();
       

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Category/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Category}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
