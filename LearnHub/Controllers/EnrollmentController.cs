 
using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

namespace LearnHub.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(EnrollmentService enrollmentService, UserManager<ApplicationUser> usermanager,
            IConfiguration configuration)
        {
            _enrollmentService = enrollmentService;
            _usermanager = usermanager;
            _configuration = configuration;

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }
 

    
        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var enrollments = await _enrollmentService.GetBookedCourses(user.Id); 

            return View(enrollments);
        }

     
        [HttpPost]
        public async Task<IActionResult> BuyCourse(int courseId)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var course = await _enrollmentService.FindCourse(courseId);
              
            if (course == null)
                return NotFound();

     
            var existingEnrollment = await _enrollmentService.CheckExistingEnrollment(courseId,user.Id);

            if (existingEnrollment != null)
            {
                TempData["Error"] = "You are already enrolled in this course!";
                return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
            }

      
            var domain = $"{Request.Scheme}://{Request.Host}";

            
            var imageUrl = course.CoverImageUrl;
            if (!string.IsNullOrEmpty(imageUrl) && !imageUrl.StartsWith("http"))
            {
                imageUrl = $"{domain}{course.CoverImageUrl}";
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "egp",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = course.Title,
                                Description = course.Description.Length > 500
                                    ? course.Description.Substring(0, 500)
                                    : course.Description,
                            },
                            UnitAmount = course.Price * 100,  
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = $"{domain}/Enrollment/PaymentSuccess?courseId={courseId}",
                CancelUrl = $"{domain}/Course/Details/{courseId}",
                Metadata = new Dictionary<string, string>
                {
                    { "courseId", courseId.ToString() },
                    { "userId", user.Id }
                }
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Redirect(session.Url);
        }

       
        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int courseId)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var course = await _enrollmentService.FindCourse(courseId);
            if (course == null)
                return NotFound();

            
            var existingEnrollment =   await _enrollmentService.CheckExistingEnrollment(courseId, user.Id); 

            if (existingEnrollment == null)
            {
                
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    ApplicationUserId = user.Id,
                    EnrolledAt = DateTime.Now
                };

               await _enrollmentService.BuyCourse(courseId,user.Id); 
            }

            TempData["Message"] = "Course purchased successfully! You can now start learning.";
            return RedirectToAction("MyCourses", "Enrollment", new { id = courseId });
        }

        
        [HttpPost]
        public async Task<IActionResult> AddRating(int courseId, int rating)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

           
            var enrollment = await _enrollmentService.CheckExistingEnrollment(courseId, user.Id);

            if (enrollment == null)
            {
                TempData["Error"] = "You must purchase the course before rating it!";
                return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
            }


            if (rating < 1 || rating > 5)
            {
                TempData["Error"] = "Rating must be between 1 and 5!";
                return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
            }

            var course = await _enrollmentService.FindCourse(courseId);
            if (course == null)
                return NotFound();

          
           await _enrollmentService.AddRating(courseId,user.Id, rating);

            TempData["Message"] = "Your rating has been submitted successfully!";
            return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
        }

        
        [HttpPost]
        public async Task<IActionResult> RemoveRating(int courseId)
        {
            var user = await _usermanager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var enrollment = await _enrollmentService.CheckExistingEnrollment(courseId, user.Id);

            if (enrollment == null || !enrollment.Rating.HasValue)
            {
                TempData["Error"] = "No rating found to remove!";
                return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
            }

            var course = await _enrollmentService.FindCourse(courseId);
            if (course == null)
                return NotFound();

          
           await _enrollmentService.RemoveRating(courseId,user.Id);

            TempData["Message"] = "Your rating has been removed!";
            return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
        }
    }
}


















