using LearnHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Domain.Interfaces
{
    public interface ILessonRepository
    {
      
        Task<Lesson> AddAsync(Lesson lesson);

        Task<Lesson> UpdateAsync(Lesson lesson);

        Task<bool> DeleteAsync(int id);

        Task<List<Lesson>> LessonsByCourseAsync(int courseId);
        Task<List<Lesson>> LessonsByUserAsync(string userId);
       
        Task<List<Course>> GetUserCoursesAsync(string userId);
        Task<Lesson?> GetByIdAsync(int id);
        Task<Course?> GetCourseWithLessonsAsync(int courseId);
        Task<bool> CheckEnrollmentAsync(int courseId, string userId);

    }
}
