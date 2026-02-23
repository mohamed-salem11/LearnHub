using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LearnHub.Application.Services.Commands.Lessons;
using LearnHub.Application.Services.Queries.Lessons;

namespace LearnHub.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public LessonController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            var lessons = courseId.HasValue
                ? await _mediator.Send(new GetLessonsByCourseQuery(courseId.Value))
                : await _mediator.Send(new GetLessonsByUserQuery(user.Id));
            return View(lessons);
        }

        [HttpGet]
        public async Task<IActionResult> LessonsByCourse(int id)
        {
            var lessons = await _mediator.Send(new GetLessonsByCourseQuery(id));
            var course = await _mediator.Send(new GetCourseWithLessonsQuery(id));
            if (course == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            bool isOwner = course.ApplicationUserId == userId;
            bool isEnrolled = await _mediator.Send(new CheckEnrollmentQuery(id, userId));

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
            var lesson = await _mediator.Send(new GetLessonByIdQuery(id.Value));
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        public async Task<IActionResult> Create(int? courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor) return Forbid();

            var userCourses = await _mediator.Send(new GetUserCoursesQuery(user.Id));
            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", courseId);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson, IFormFile videoFile)
        {
            var result = await _mediator.Send(new CreateLessonCommand(lesson, videoFile));
            if (!result.Success)
            {
                ModelState.AddModelError("videoFile", result.Error);
                var user = await _userManager.GetUserAsync(User);
                var userCourses = await _mediator.Send(new GetUserCoursesQuery(user.Id));
                ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", lesson.CourseId);
                return View(lesson);
            }

            using (var stream = new FileStream(result.FilePath!, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            return RedirectToAction("LessonsByCourse", "Lesson", new { id = result.Data!.CourseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var lesson = await _mediator.Send(new GetLessonByIdQuery(id));
            if (lesson == null) return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (lesson.Course.ApplicationUserId != user.Id) return Forbid();

            var userCourses = await _mediator.Send(new GetUserCoursesQuery(user.Id));
            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", lesson.CourseId);
            return View(lesson);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Lesson lesson, IFormFile? videoFile)
        {
            var result = await _mediator.Send(new UpdateLessonCommand(lesson, videoFile));
            if (!result.Success)
            {
                ModelState.AddModelError("videoFile", result.Error ?? "Update failed");
                return View(lesson);
            }
            return RedirectToAction("LessonsByCourse", new { id = result.Data!.CourseId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await _mediator.Send(new GetLessonByIdQuery(id));
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _mediator.Send(new GetLessonByIdQuery(id));
            if (lesson == null) return NotFound();

            var result = await _mediator.Send(new DeleteLessonCommand(id));
            if (!result) return NotFound();

            return RedirectToAction("LessonsByCourse", "Lesson", new { id = lesson.CourseId });
        }
    }
}
