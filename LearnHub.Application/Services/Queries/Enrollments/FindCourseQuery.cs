using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public record FindCourseQuery(int CourseId) : IRequest<Course?>;
}
