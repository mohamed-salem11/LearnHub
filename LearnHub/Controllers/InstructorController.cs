  
using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Controllers
{
    public class InstructorController : Controller
    {
        private readonly InstructorService _instructorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorController(   InstructorService instructorService, UserManager<ApplicationUser> userManager)
        {
            _instructorService = instructorService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var instructor =await _instructorService.GetInstructorPage(id);

            if (instructor == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.IsOwner = currentUser != null && currentUser.Id == instructor.Id;

            return View("Profile", instructor);
        }

 
        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor)
                return Forbid();

            var instructor =await _instructorService.GetInstructorPage(id: user.Id);


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

            try
            {
                await _instructorService.EditProfile(user.Id, model, PhotoFile);
                return RedirectToAction("MyProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong while Updating Profile.");
                return View(model);
            }
          
        }



    }
}













