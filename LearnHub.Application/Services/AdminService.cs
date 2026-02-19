using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;

namespace LearnHub.Application.Services
{
    public class AdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

   
        public async Task<List<ApplicationUser>> GetInstructorRequestsAsync()
        {
            return await _adminRepository.GetInstructorRequestsAsync();
        }

      
        public async Task ApproveInstructorAsync(string userId)
        {
            await _adminRepository.ApproveInstructorAsync(userId);
        }

    
        public async Task RejectInstructorAsync(string userId)
        {
            await _adminRepository.RejectInstructorAsync(userId);
        }
         
        public async Task<List<Course>> GetPendingCoursesAsync()
        {
           
            return await _adminRepository.GetPendingCoursesAsync();
        }
 
        public async Task ApproveCourseAsync(int id)
        {
            await _adminRepository.ApproveCourseAsync(id);
        }
 
        public async Task RejectCourseAsync(int id)
        {
            await _adminRepository.RejectCourseAsync(id);
        }

    
        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await _adminRepository.FindCourseByIdAsync(id);
        }
    }
}