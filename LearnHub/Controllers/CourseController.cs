using LearnHub.Data;
using LearnHub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnHub.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _usermanager;

        public CourseController(ApplicationDbContext context, UserManager<ApplicationUser> usermanager)
        {
            _context = context;
            _usermanager = usermanager;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .Where(c => c.Status == CourseStatus.Approved);

            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }


        [HttpGet]
        public async Task<IActionResult> CoursesByCategory(int id)
        {
            var courses = await _context.Courses
         .Include(c => c.ApplicationUser)
         .Include(c => c.Category)
         .Where(c => c.CategoryId == id&& c.Status == CourseStatus.Approved).ToListAsync();



            return View(courses);
        }

        public IActionResult SearchPage()
        {
            return View();
        }
        public IActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("Index"); 

            var courses = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.ApplicationUser)
                .Include(c => c.Enrollments)
                .Where(c =>
                    c.Title.Contains(query) ||                  
                    c.Description.Contains(query) ||                
                    c.Category.Name.Contains(query) ||                
                    c.ApplicationUser.FullName.Contains(query)         
                )
                .ToList();

            return View("SearchResults", courses);
        }



        [HttpPost]
        public async Task<IActionResult> Create(Course course, IFormFile imageFile)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    ModelState.AddModelError("imageFile", "Please upload an image.");
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
                    return View(course);
                }

                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                string[] allowedMimeTypes = { "image/jpeg", "image/jpg", "image/png" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(imageFile.ContentType.ToLower()))
                {
                    ModelState.AddModelError("imageFile", "Only JPG, JPEG, PNG files are allowed.");
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
                    return View(course);
                }

                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                course.CoverImageUrl = $"/uploads/{fileName}";

                var user = await _usermanager.GetUserAsync(User);
                course.ApplicationUserId = user.Id;

                course.TotalRating = 0;
                course.TotalVotes = 0;
                course.NumberOfLearnears = 0;
                course.Status = CourseStatus.Draft;

                _context.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction("LessonsByCourse", "Lesson", new { id = course.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);
                return View(course);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestReview(int courseId)
        {
            var user = await _usermanager.GetUserAsync(User);

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId && c.ApplicationUserId == user.Id);

            if (course == null)
                return Forbid();

            course.Status = CourseStatus.Pending;
            _context.Update(course);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Review request submitted successfully ";

            return RedirectToAction("Profile", "Instructor", new { id = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course updatedCourse, IFormFile CoverImageFile)
        {
            if (id != updatedCourse.Id)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            
            course.Title = updatedCourse.Title;
            course.Description = updatedCourse.Description;
            course.Price = updatedCourse.Price;
            course.CategoryId = updatedCourse.CategoryId;

            
            if (CoverImageFile != null && CoverImageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(CoverImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await CoverImageFile.CopyToAsync(stream);
                }

                course.CoverImageUrl = "/uploads/" + uniqueFileName;
            }

            _context.Update(course);
            await _context.SaveChangesAsync();

          
            return RedirectToAction("MyProfile", "Instructor");
        }



        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
