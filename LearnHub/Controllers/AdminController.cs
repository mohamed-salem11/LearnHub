using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnHub.Application.Services.Commands.Admin;
using LearnHub.Application.Services.Queries.Admin;

namespace LearnHub.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> InstructorRequests()
        {
            var requests = await _mediator.Send(new GetInstructorRequestsQuery());
            return View(requests);
        }

        public async Task<IActionResult> ApproveInstructor(string userId)
        {
            await _mediator.Send(new ApproveInstructorCommand(userId));
            TempData["Message"] = "Instructor approved successfully.";
            return RedirectToAction(nameof(InstructorRequests));
        }

        public async Task<IActionResult> RejectInstructor(string userId)
        {
            await _mediator.Send(new RejectInstructorCommand(userId));
            TempData["Message"] = "Instructor request rejected.";
            return RedirectToAction(nameof(InstructorRequests));
        }

        public async Task<IActionResult> PendingCourses()
        {
            var courses = await _mediator.Send(new GetPendingCoursesQuery());
            return View(courses);
        }

        public async Task<IActionResult> ApproveCourse(int id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            if (course == null) return NotFound();

            await _mediator.Send(new ApproveCourseCommand(id));
            TempData["Message"] = "Course approved successfully.";
            return RedirectToAction(nameof(PendingCourses));
        }

        public async Task<IActionResult> RejectCourse(int id)
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(id));
            if (course == null) return NotFound();

            await _mediator.Send(new RejectCourseCommand(id));
            TempData["Message"] = "Course rejected successfully.";
            return RedirectToAction(nameof(PendingCourses));
        }
    }
}
