using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public record RejectCourseCommand(int CourseId) : IRequest<Unit>;
}
