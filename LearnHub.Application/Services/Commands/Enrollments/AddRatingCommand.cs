using MediatR;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    public record AddRatingCommand(int CourseId, string UserId, int Rating) : IRequest<Unit>;
}
