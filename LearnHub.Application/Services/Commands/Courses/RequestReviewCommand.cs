using MediatR;

namespace LearnHub.Application.Services.Commands.Courses
{
    public record RequestReviewCommand(int CourseId, string UserId) : IRequest<bool>;
}
