using LearnHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Domain.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetBookedCourses(string userId);
        Task BuyCourse(int courseId, string userId);
        Task<Course?> FindCourse(int courseId);
        Task <Enrollment?>CheckExistingEnrollment(int courseId,string userId);
        Task AddRating(int courseId, string userId, int rating);
        Task UpdateRating(int courseId, string userId,int rating);
        Task RemoveRating(int courseId, string userId);

    }
}
