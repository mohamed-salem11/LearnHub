using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var instructor = await _userManager.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsInstructor);

            if (instructor == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.IsOwner = currentUser != null && currentUser.Id == instructor.Id;

            return View("Profile", instructor);
        }


        //  [HttpGet("Instructor/MyProfile")]
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor)
                return Forbid();

            var instructor = await _userManager.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.Id == user.Id);


            ViewBag.IsOwner = true;
            return View("Profile", instructor);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor)
                return Forbid();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser model, IFormFile PhotoFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor)
                return Forbid();


            user.Bio = model.Bio;
            user.Specialization = model.Specialization;
            user.IsInstructorRequestPending = true;

            if (PhotoFile != null && PhotoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(PhotoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await PhotoFile.CopyToAsync(fileStream);
                }

                user.Photo = "/uploads/" + uniqueFileName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Something went wrong while Updating Profile.");
                return View(model);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("MyProfile");
        }



    }
}













