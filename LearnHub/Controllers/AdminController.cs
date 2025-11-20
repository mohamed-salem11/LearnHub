using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // طلبات الانضمام كمُدرّس
        public async Task<IActionResult> InstructorRequests()
        {
            var requests = await _userManager.Users
                .Where(u => u.IsInstructorRequestPending && !u.IsInstructor)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> ApproveInstructor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.IsInstructor = true;
            user.IsInstructorRequestPending = false;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Instructor approved successfully.";
            return RedirectToAction("InstructorRequests");
        }

        public async Task<IActionResult> RejectInstructor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            user.IsInstructorRequestPending = false;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Instructor request rejected.";
            return RedirectToAction("InstructorRequests");
        }

        // الكورسات غير المقبولة
        public async Task<IActionResult> PendingCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .Where(c => c.Status == CourseStatus.Pending)
                .ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> ApproveCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Status = CourseStatus.Approved;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Course approved successfully.";
            return RedirectToAction("PendingCourses");
        }

        public async Task<IActionResult> RejectCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            course.Status = CourseStatus.Rejected;
            _context.Update(course);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Course rejected successfully.";
            return RedirectToAction("PendingCourses");
        }

    }
}








