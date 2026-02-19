using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Infrastructure.Repositories
{
    public class EfAdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public EfAdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationUser>> GetInstructorRequestsAsync()
        {
            return await _context.ApplicationUsers
                .Where(u => u.IsInstructorRequestPending && !u.IsInstructor)
                .ToListAsync();
        }

        public async Task ApproveInstructorAsync(string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);
            if (user != null)
            {
                user.IsInstructor = true;
                user.IsInstructorRequestPending = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectInstructorAsync(string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);
            if (user != null)
            {
                user.IsInstructorRequestPending = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Course>> GetPendingCoursesAsync()
        {
            return await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .Where(c => c.Status == CourseStatus.Pending)
                .ToListAsync();
        }

        public async Task ApproveCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.Status = CourseStatus.Approved;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RejectCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                course.Status = CourseStatus.Rejected;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Course?> FindCourseByIdAsync(int id)
        {
            return await _context.Courses.FindAsync(id);
        }
    }
}