using MediatR;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    
    public class BuyCourseCommand : IRequest<Unit>
    {
        public int CourseId { get; }
        public string UserId { get; }

        public BuyCourseCommand(int courseId, string userId)
        {
            CourseId = courseId;
            UserId = userId;
        }
    }
}
