using Microsoft.AspNetCore.Identity;

namespace LearnHub.Domain.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string? Bio { get; set; }
        public string? Photo { get; set; }
        public bool IsInstructor { get; set; } = false;
        public bool IsInstructorRequestPending { get; set; } = false;
        public string? Specialization { get; set; }   
        public virtual List<Course>? Courses { get; set; }
        public virtual List<Enrollment>? Enrollments { get; set; }  
    }
}






















