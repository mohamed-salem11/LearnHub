using MediatR;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    public record RemoveRatingCommand(int CourseId, string UserId) : IRequest<Unit>;
}
