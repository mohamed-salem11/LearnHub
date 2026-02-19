using LearnHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCourses();
        Task<List<Course>> CoursesByCategory(int id);
        Task<List<Course>> Search(string query);
        Task Update(Course course);
        Task<Course> Add(Course course);
        Task Delete(int id);
        Task<bool> RequestReview(int id, string userId);

        Task<Course?> Find(int id);
        Task<List<Category>> GetCategories();
    }
}
