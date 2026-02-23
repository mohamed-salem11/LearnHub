using MediatR;

namespace LearnHub.Application.Services.Commands.Courses
{
    public record DeleteCourseCommand(int Id) : IRequest<Unit>;
}
