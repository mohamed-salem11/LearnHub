using LearnHub.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> InstructorRequests()
        {
            var requests = await _adminService.GetInstructorRequestsAsync();
            return View(requests);
        }

        public async Task<IActionResult> ApproveInstructor(string userId)
        {
            await _adminService.ApproveInstructorAsync(userId);
            TempData["Message"] = "Instructor approved successfully.";
            return RedirectToAction(nameof(InstructorRequests));
        }

        public async Task<IActionResult> RejectInstructor(string userId)
        {
            await _adminService.RejectInstructorAsync(userId);
            TempData["Message"] = "Instructor request rejected.";
            return RedirectToAction(nameof(InstructorRequests));
        }

        public async Task<IActionResult> PendingCourses()
        {
            var courses = await _adminService.GetPendingCoursesAsync();
            return View(courses);
        }

        public async Task<IActionResult> ApproveCourse(int id)
        {
            var course = await _adminService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            await _adminService.ApproveCourseAsync(id);
            TempData["Message"] = "Course approved successfully.";
            return RedirectToAction(nameof(PendingCourses));
        }

        public async Task<IActionResult> RejectCourse(int id)
        {
            var course = await _adminService.GetCourseByIdAsync(id);
            if (course == null) return NotFound();

            await _adminService.RejectCourseAsync(id);
            TempData["Message"] = "Course rejected successfully.";
            return RedirectToAction(nameof(PendingCourses));
        }
    }
}