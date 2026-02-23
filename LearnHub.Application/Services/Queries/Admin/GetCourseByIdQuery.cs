using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Admin
{
    public record GetCourseByIdQuery(int CourseId) : IRequest<Course?>;
}
