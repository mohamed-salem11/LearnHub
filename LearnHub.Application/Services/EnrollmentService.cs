using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearnHub.Application.Services
{
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<List<Enrollment>> GetBookedCourses(string userId)
        {
            return await _enrollmentRepository.GetBookedCourses(userId);
        }

        public async Task BuyCourse(int courseId, string userId)
        {
            await _enrollmentRepository.BuyCourse(courseId,userId);
        }

        public async Task<Course?> FindCourse(int courseId)
        {
            return await _enrollmentRepository.FindCourse(courseId);
        }

        public async Task<Enrollment?> CheckExistingEnrollment(int courseId, string userId)
        {
            return await _enrollmentRepository.CheckExistingEnrollment(courseId, userId);
        }

        public async Task AddRating(int courseId, string userId, int rating)
        {
            await _enrollmentRepository.AddRating(courseId,  userId, rating);
        }

        public async Task UpdateRating(int courseId, string userId,int rating)
        {
            await _enrollmentRepository.UpdateRating(courseId, userId,rating);
        }

        public async Task RemoveRating(int courseId,string userId)
        {
            await _enrollmentRepository.RemoveRating(courseId,userId);
        }
    }
}
