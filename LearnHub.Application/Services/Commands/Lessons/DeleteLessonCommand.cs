using MediatR;

namespace LearnHub.Application.Services.Commands.Lessons
{
    public record DeleteLessonCommand(int Id) : IRequest<bool>;
}
