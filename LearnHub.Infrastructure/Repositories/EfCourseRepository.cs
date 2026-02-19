using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Infrastructure.Repositories
{
    public class EfCourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public EfCourseRepository(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<List<Course>> CoursesByCategory(int id)
        {
            var courses = await _context.Courses
                 .Include(c => c.ApplicationUser)
                 .Include(c => c.Category)
                 .Where(c => c.Status == CourseStatus.Approved && c.CategoryId == id).ToListAsync();
            return courses;
        }

        public async Task<List<Course>> GetCourses()
        {
            var courses = await _context.Courses
            .Include(c => c.ApplicationUser)
            .Include(c => c.Category)
            .Where(c => c.Status == CourseStatus.Approved).ToListAsync();
            return courses;
        }
        public async Task<List<Course>> Search(string query)
        {
            var courses = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.ApplicationUser)
                .Include(c => c.Enrollments)
                .Where(c =>
                    c.Status == CourseStatus.Approved && (
                    c.Title.Contains(query) ||
                    c.Description.Contains(query) ||
                    c.Category.Name.Contains(query) ||
                    c.ApplicationUser.FullName.Contains(query)))
                .ToListAsync();

            return courses;
        }

        public async Task<Course> Add(Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return course;

        }
        public async Task Update(Course course)
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Remove(course);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Course?> Find(int id)
        {
            return await _context.Courses
                .Include(c => c.ApplicationUser)
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> RequestReview(int id, string userId)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    c.ApplicationUserId == userId);

            if (course == null)
                return false;

            course.Status = CourseStatus.Pending;

            _context.Courses.Update(course);

            await _context.SaveChangesAsync();

            return true;
        }
         

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();

        }
    }
}