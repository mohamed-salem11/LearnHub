using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    [Authorize]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public LessonController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        // GET: Lesson/Index
        public async Task<IActionResult> Index(int? courseId)
        {
            var user = await _usermanager.GetUserAsync(User);

            IQueryable<Lesson> lessons = _context.Lessons
                .Include(l => l.Course)
                .ThenInclude(c => c.ApplicationUser);

            if (courseId.HasValue)
            {
                lessons = lessons.Where(l => l.CourseId == courseId.Value);
                ViewBag.CourseId = courseId;
                var course = await _context.Courses.FindAsync(courseId);
                ViewBag.CourseName = course?.Title;
            }
            else
            {
                lessons = lessons.Where(l => l.Course.ApplicationUserId == user.Id);
            }

            return View(await lessons.OrderBy(l => l.Id).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> LessonsByCourse(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            ViewBag.CourseId = course.Id;
            ViewBag.CourseName = course.Title;
            ViewBag.CourseOwnerId = course.ApplicationUserId;
            ViewBag.CurrentUserId = _usermanager.GetUserId(User);

            return View("Index", course.Lessons.ToList());
        }


        // GET: Lesson/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .ThenInclude(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lesson == null)
                return NotFound();

            return View(lesson);
        }

        // GET: Lesson/Create
        public async Task<IActionResult> Create(int? courseId)
        {
            var user = await _usermanager.GetUserAsync(User);

            var userCourses = await _context.Courses
                .Where(c => c.ApplicationUserId == user.Id)
                .ToListAsync();

            if (courseId.HasValue)
            {
                ViewBag.SelectedCourseId = courseId;
            }

            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", courseId);
            return View();
        }

        // POST: Lesson/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lesson lesson, IFormFile videoFile)
        {
            var user = await _usermanager.GetUserAsync(User);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == lesson.CourseId && c.ApplicationUserId == user.Id);

            if (course == null)
            {
                ModelState.AddModelError("", "You can only add lessons to your own courses.");
                ViewData["CourseId"] = new SelectList(_context.Courses.Where(c => c.ApplicationUserId == user.Id), "Id", "Title");
                return View(lesson);
            }

            if (videoFile == null || videoFile.Length == 0)
            {
                ModelState.AddModelError("videoFile", "Please upload a video file.");
                ViewData["CourseId"] = new SelectList(_context.Courses.Where(c => c.ApplicationUserId == user.Id), "Id", "Title", lesson.CourseId);
                return View(lesson);
            }

            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
            string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
            var fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(videoFile.ContentType.ToLower()))
            {
                ModelState.AddModelError("videoFile", "Only video files (MP4, AVI, MOV, MKV, WEBM) are allowed.");
                ViewData["CourseId"] = new SelectList(_context.Courses.Where(c => c.ApplicationUserId == user.Id), "Id", "Title", lesson.CourseId);
                return View(lesson);
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await videoFile.CopyToAsync(stream);
            }

            lesson.VideoUrl = $"/uploads/videos/{fileName}";

            _context.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { courseId = lesson.CourseId });
        }

        // GET: Lesson/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
                return NotFound();

            var user = await _usermanager.GetUserAsync(User);

            if (lesson.Course.ApplicationUserId != user.Id)
                return Forbid();

            var userCourses = await _context.Courses
                .Where(c => c.ApplicationUserId == user.Id)
                .ToListAsync();

            ViewData["CourseId"] = new SelectList(userCourses, "Id", "Title", lesson.CourseId);
            return View(lesson);
        }

        // POST: Lesson/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lesson lesson, IFormFile videoFile)
        {
            if (id != lesson.Id)
                return NotFound();

            var user = await _usermanager.GetUserAsync(User);
            var course = await _context.Courses.FindAsync(lesson.CourseId);

            if (course == null || course.ApplicationUserId != user.Id)
            {
                ModelState.AddModelError("", "Invalid course selection.");
                ViewData["CourseId"] = new SelectList(_context.Courses.Where(c => c.ApplicationUserId == user.Id), "Id", "Title");
                return View(lesson);
            }

            var existingLesson = await _context.Lessons.FindAsync(id);
            if (existingLesson == null)
                return NotFound();

            existingLesson.Title = lesson.Title;
            existingLesson.CourseId = lesson.CourseId;

            if (videoFile != null && videoFile.Length > 0)
            {
                string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
                string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
                var fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(videoFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("videoFile", "Only video files are allowed.");
                    ViewData["CourseId"] = new SelectList(_context.Courses.Where(c => c.ApplicationUserId == user.Id), "Id", "Title");
                    return View(lesson);
                }

                if (!string.IsNullOrEmpty(existingLesson.VideoUrl))
                {
                    var oldVideoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingLesson.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldVideoPath))
                    {
                        System.IO.File.Delete(oldVideoPath);
                    }
                }

                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                existingLesson.VideoUrl = $"/uploads/videos/{fileName}";
            }

            try
            {
                _context.Update(existingLesson);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LessonExists(lesson.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index), new { courseId = lesson.CourseId });
        }

        // GET: Lesson/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lesson == null)
                return NotFound();

            var user = await _usermanager.GetUserAsync(User);
            if (lesson.Course.ApplicationUserId != user.Id)
                return Forbid();

            return View(lesson);
        }

        // POST: Lesson/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson != null)
            {
                if (!string.IsNullOrEmpty(lesson.VideoUrl))
                {
                    var videoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", lesson.VideoUrl.TrimStart('/'));
                    if (System.IO.File.Exists(videoPath))
                    {
                        System.IO.File.Delete(videoPath);
                    }
                }

                var courseId = lesson.CourseId;
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { courseId });
            }

            return RedirectToAction(nameof(Index));
        }

        private bool LessonExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }
    }
}