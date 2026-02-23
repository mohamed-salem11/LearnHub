using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearnHub.Application.Services.Commands.Instructor;
using LearnHub.Application.Services.Queries.Instructor;

namespace LearnHub.Controllers
{
    public class InstructorController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorController(IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var instructor = await _mediator.Send(new GetInstructorPageQuery(id));
            if (instructor == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.IsOwner = currentUser != null && currentUser.Id == instructor.Id;

            return View("Profile", instructor);
        }

        public async Task<IActionResult> MyProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor) return Forbid();

            var instructor = await _mediator.Send(new GetInstructorPageQuery(user.Id));
            ViewBag.IsOwner = true;
            return View("Profile", instructor);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor) return Forbid();

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser model, IFormFile PhotoFile)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsInstructor) return Forbid();

            try
            {
                await _mediator.Send(new EditProfileCommand(user.Id, model, PhotoFile));
                return RedirectToAction("MyProfile");
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong while Updating Profile.");
                return View(model);
            }
        }
    }
}
