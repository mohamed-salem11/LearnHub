using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace LearnHub.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly LessonService _lessonService;
        private readonly UserManager<ApplicationUser> _userManager;
    

        public LessonController(LessonService lessonService, UserManager<ApplicationUser> userManager )
        {
            _lessonService = lessonService;
            _userManager = userManager;
         
        }

        public async Task<IActionResult> Index(int? courseId)
        {
            var user = await _userManager.GetUserAsync(User);

            var lessons = courseId.HasValue
                ? await _lessonService.LessonsByCourseAsync(courseId.Value)
                : await _lessonService.LessonsByUserAsync(user.Id);  

            return View(lessons);
        }


        [HttpGet]
        public async Task<IActionResult> LessonsByCourse(int id)
        {
            var lessons = await _lessonService.LessonsByCourseAsync(id);

            var course = await _lessonService.GetCourseWithLessonsAsync(id); 
            if (course == null) return NotFound();
            var userId = _userManager.GetUserId(User); 
            bool isOwner = course.ApplicationUserId == userId;
            bool isEnrolled = await _lessonService.CheckEnrollmentAsync(id, userId);
            ViewBag.IsOwner = isOwner;
            ViewBag.IsEnrolled = isEnrolled; 
            ViewBag.CourseId = id;
            ViewBag.CourseName = course.Title;
            ViewBag.CourseOwnerId = course.ApplicationUserId; 
            ViewBag.CurrentUserId = userId;
            return View("Index", course.Lessons.OrderBy(l => l.Id).ToList());
        }
        

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var lesson = await _lessonService.GetByIdAsync(id.Value);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        public async Task<IActionResult> Create(int? courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor) return Forbid();
            var userCourses = await _lessonService.GetUserCoursesAsync(user.Id);
            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", courseId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson, IFormFile videoFile)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _lessonService.CreateLessonAsync(lesson, videoFile);

            if (!result.Success)
            {
                ModelState.AddModelError("videoFile", result.Error);
                var userCourses = await _lessonService.GetUserCoursesAsync(user.Id);
                ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", lesson.CourseId);
               return View(lesson); 
            }

            
            using (var stream = new FileStream(result.FilePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            return RedirectToAction("LessonsByCourse", "Lesson", new { id = result.Data.CourseId });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (lesson.Course.ApplicationUserId != user.Id)
                return Forbid();

            var userCourses = await _lessonService.GetUserCoursesAsync(user.Id);
            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", lesson.CourseId);

            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Lesson lesson, IFormFile? videoFile)
        {
            var result = await _lessonService.UpdateLessonAsync(lesson, videoFile);

            if (!result.Success)
            {
                ModelState.AddModelError("videoFile", result.Error ?? "Update failed");
                return View(lesson);
            }

            return RedirectToAction("LessonsByCourse", new { id = result.Data.CourseId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _lessonService.GetByIdAsync(id);
            if (lesson == null) return NotFound();

            var result = await _lessonService.DeleteAsync(id);
            if (!result) return NotFound();

            return RedirectToAction("LessonsByCourse", "Lesson", new { id = lesson.CourseId });
        }

 
    }
}
