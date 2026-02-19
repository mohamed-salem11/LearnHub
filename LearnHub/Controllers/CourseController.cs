
using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnHub.Controllers
{
    public class CourseController : Controller
    {
 
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly CourseService _courseService;

        public CourseController(CourseService courseService, UserManager<ApplicationUser> usermanager)
        {
            _courseService = courseService;
            _usermanager = usermanager;
        }
        public async Task<IActionResult> Index()
        {
           var courses=await _courseService.GetCourses();

            return View(courses);
        }


        [HttpGet]
        public async Task<IActionResult> CoursesByCategory(int id)
        {
            var courses =await  _courseService.CoursesByCategory(id);
            return View(courses);
        }

        public IActionResult SearchPage()
        {
            return View();
        }
        public async Task<IActionResult> Search(string query)
        {
            if (string.IsNullOrEmpty(query))
                return RedirectToAction("Index");

            var courses = await _courseService.Search(query);

            return View("SearchResults", courses);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _courseService.GetCategories(), "Id", "Name");
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(Course course, IFormFile imageFile)
        {
            var userId = _usermanager.GetUserId(User);
            var result = await _courseService.Add(course, imageFile,userId);
            if (!result.Success)
            {
                ModelState.AddModelError("imageFile", result.Error!);
                ViewData["CategoryId"] = new SelectList(await _courseService.GetCategories(), "Id", "Name", course.CategoryId);
                return View(course);
            }
           return RedirectToAction("LessonsByCourse", "Lesson", new { id = course.Id });

        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseService.Find(id);
            if (course == null) 
            {
                return NotFound();
            }

            var categories = await _courseService.GetCategories();

            ViewData["CategoryId"] =
                new SelectList(categories, "Id", "Name", course.CategoryId);

            return View(course);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> Edit(int id, Course updatedCourse, IFormFile CoverImageFile)
        {      
            if (id != updatedCourse.Id) return NotFound();
 
                await _courseService.Update(id, updatedCourse, CoverImageFile);
                return RedirectToAction("MyProfile", "Instructor");
     
        }


        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.Find(id);
            if (course == null)
            {
                TempData["Error"] = "Course not found or already deleted.";
                return RedirectToAction("Index");
            }

            return View(course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseService.Find(id);
            if (course != null)
            {
                if (User.IsInRole("Admin") || course.ApplicationUserId == _usermanager.GetUserId(User))
                {
                    await _courseService.Delete(id);
                }
                else
                {
                    return Forbid();
                }
            }

            if (User.IsInRole("Admin"))
            return Redirect(Request.Headers["Referer"].ToString());
            else 
            return RedirectToAction("MyProfile", "Instructor");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestReview(int courseId)
        {
            var user = await _usermanager.GetUserAsync(User);

            var success = await _courseService.RequestReview(courseId, user.Id);

            if (!success)
                return Forbid();

            TempData["Message"] = "Review request submitted successfully";
            return RedirectToAction("Profile", "Instructor", new { id = user.Id });
        }





    }
}
