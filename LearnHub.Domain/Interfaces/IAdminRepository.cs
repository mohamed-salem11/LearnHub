using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnHub.Domain.Entities;
namespace LearnHub.Domain.Interfaces
{
    public interface IAdminRepository
    {
        Task<List<ApplicationUser>> GetInstructorRequestsAsync();
        Task<List<Course>> GetPendingCoursesAsync();
        Task ApproveInstructorAsync(string userId);
        Task RejectInstructorAsync(string userId);
        Task ApproveCourseAsync(int id);
        Task RejectCourseAsync(int id);
        Task<Course?> FindCourseByIdAsync(int id);  
    }
}
