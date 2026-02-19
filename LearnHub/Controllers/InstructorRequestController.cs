
using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Infrastructure.Persistence;
using LearnHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Controllers
{
    public class InstructorRequestController : Controller
    {
        private readonly InstructorRequestService _InstructorRequestService;
        private readonly UserManager<ApplicationUser> _usermanager;
       
        public InstructorRequestController(InstructorRequestService instructorrequestservice, UserManager<ApplicationUser> usermanager)
        {
            _InstructorRequestService = instructorrequestservice;
            _usermanager = usermanager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult ConfirmJoin()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult SubmitRequest()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitRequest(ApplicationUser model, IFormFile PhotoFile)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            user.Bio = model.Bio;
            user.Specialization = model.Specialization;
            user.IsInstructorRequestPending = true;

            var result = await _InstructorRequestService.SubmitRequestAsync(user, PhotoFile);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Error ?? "Something went wrong.");
                return View(model);
            }

            TempData["Message"] = "Your instructor request has been submitted and is pending admin approval.";
            return RedirectToAction("Index", "Category");
        }


    }
}












