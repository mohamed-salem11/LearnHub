using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public record CheckExistingEnrollmentQuery(int CourseId, string UserId) : IRequest<Enrollment?>;
}
