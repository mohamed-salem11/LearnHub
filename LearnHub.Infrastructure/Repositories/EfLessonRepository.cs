using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LearnHub.Infrastructure.Repositories
{
    public class EfLessonRepository : ILessonRepository
    {
        private readonly ApplicationDbContext _context;

        public EfLessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Lesson> AddAsync(Lesson lesson)
        {
            await _context.Lessons.AddAsync(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson> UpdateAsync(Lesson lesson)
        {
            _context.Lessons.Update(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lesson>> LessonsByCourseAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            return course?.Lessons.OrderBy(l => l.Id).ToList() ?? new List<Lesson>();
        }

        public async Task<List<Lesson>> LessonsByUserAsync(string userId)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .Where(l => l.Course.ApplicationUserId == userId)
                .OrderBy(l => l.Id)
                .ToListAsync();
        }
        public async Task<List<Course>> GetUserCoursesAsync(string userId)
        {
            return await _context.Courses
                .Where(c => c.ApplicationUserId == userId)
                .ToListAsync();
        }
        public async Task<Course?> GetCourseWithLessonsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Lessons)
                .Include(c => c.ApplicationUser)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }
        public async Task<bool> CheckEnrollmentAsync(int courseId, string userId)
        { 
            return await _context.Enrollments.AnyAsync(e => e.CourseId == courseId && e.ApplicationUserId == userId);
        }
        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons
                .Include(l => l.Course)
                .ThenInclude(c => c.ApplicationUser)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
