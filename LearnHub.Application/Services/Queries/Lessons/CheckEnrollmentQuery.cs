using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public record CheckEnrollmentQuery(int CourseId, string UserId) : IRequest<bool>;
}
