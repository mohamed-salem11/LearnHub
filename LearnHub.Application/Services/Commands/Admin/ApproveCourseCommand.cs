using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public record ApproveCourseCommand(int CourseId) : IRequest<Unit>;
}
