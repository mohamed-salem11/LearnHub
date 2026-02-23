using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public record GetCourseByIdQuery(int Id) : IRequest<Course?>;
}
