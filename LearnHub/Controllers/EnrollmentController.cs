using LearnHub.Data;
using LearnHub.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public EnrollmentController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;

          
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

    
        [HttpGet]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var enrollments = await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Lessons)
                .Include(e => e.Course.ApplicationUser)
                .Where(e => e.ApplicationUserId == user.Id)
                .ToListAsync();

            return View(enrollments);
        }

     
        [HttpPost]
        public async Task<IActionResult> BuyCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var course = await _context.Courses
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null)
                return NotFound();

     
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.ApplicationUserId == user.Id);

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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound();

            
            var existingEnrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.ApplicationUserId == user.Id);

            if (existingEnrollment == null)
            {
                
                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    ApplicationUserId = user.Id,
                    EnrolledAt = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);
 
                course.NumberOfLearnears++;

                await _context.SaveChangesAsync();
            }

            TempData["Message"] = "Course purchased successfully! You can now start learning.";
            return RedirectToAction("MyCourses", "Enrollment", new { id = courseId });
        }

        
        [HttpPost]
        public async Task<IActionResult> AddRating(int courseId, int rating)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

           
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.ApplicationUserId == user.Id);

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

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound();

          
            if (enrollment.Rating.HasValue)
            {
             
                course.TotalRating = course.TotalRating - enrollment.Rating.Value + rating;
                enrollment.Rating = rating;
            }
            else
            {
                
                course.TotalRating += rating;
                course.TotalVotes++;
                enrollment.Rating = rating;
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Your rating has been submitted successfully!";
            return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
        }

        
        [HttpPost]
        public async Task<IActionResult> RemoveRating(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == courseId && e.ApplicationUserId == user.Id);

            if (enrollment == null || !enrollment.Rating.HasValue)
            {
                TempData["Error"] = "No rating found to remove!";
                return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
                return NotFound();

          
            course.TotalRating -= enrollment.Rating.Value;
            course.TotalVotes--;
            enrollment.Rating = null;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Your rating has been removed!";
            return RedirectToAction("LessonsByCourse", "Lesson", new { id = courseId });
        }
    }
}


















