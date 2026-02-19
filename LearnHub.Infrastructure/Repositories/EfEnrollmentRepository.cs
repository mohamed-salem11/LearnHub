using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Infrastructure.Repositories
{
    public class EfEnrollmentRepository:IEnrollmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EfEnrollmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRating(int courseId, string userId, int rating)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if(course==null)throw new KeyNotFoundException($"Course with ID {courseId} was not found.");
            var enrollment = await CheckExistingEnrollment(  courseId , userId);

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

        }

        public async Task BuyCourse(int courseId,string userId)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) return;



            var existingEnrollment = await CheckExistingEnrollment(courseId, userId); 

            if (existingEnrollment == null)
            {

                var enrollment = new Enrollment
                {
                    CourseId = courseId,
                    ApplicationUserId = userId, 
                    EnrolledAt = DateTime.Now
                };

                _context.Enrollments.Add(enrollment);

                course.NumberOfLearnears++;

                await _context.SaveChangesAsync();
            }
        }

        public async Task <Enrollment?>CheckExistingEnrollment(int courseId, string userId)
        {
           return await _context.Enrollments
               .FirstOrDefaultAsync(e => e.CourseId == courseId && e.ApplicationUserId == userId); 
        }

        public async Task<Course?> FindCourse(int courseId)
        {

            return await _context.Courses
             .Include(c => c.ApplicationUser).FirstOrDefaultAsync(c => c.Id == courseId);

        }

        public async Task<List<Enrollment>> GetBookedCourses(string userId)
        {
           
            return await _context.Enrollments
                .Include(e => e.Course)
                    .ThenInclude(c => c.Lessons)
                .Include(e => e.Course.ApplicationUser)
                .Where(e => e.ApplicationUserId == userId) 
                .ToListAsync();
        }

        public async Task RemoveRating(int courseId, string userId)
        {
            var enrollment = await CheckExistingEnrollment(courseId, userId);
            if (enrollment == null) throw new InvalidOperationException("User is not enrolled in this course.");
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) throw new KeyNotFoundException($"Course ID {courseId} not found.");
            if (!enrollment.Rating.HasValue) return;
            course.TotalRating -= enrollment.Rating.Value;
            course.TotalVotes--;
            enrollment.Rating = null;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateRating(int courseId,string userId, int rating)
        {
            var enrollment = await CheckExistingEnrollment(courseId, userId);
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)   throw new KeyNotFoundException($"Course with ID {courseId} not found.");
            if (enrollment == null) throw new UnauthorizedAccessException("User is not enrolled in this course.");

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
        }
    }
}
