using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public record GetCourseWithLessonsQuery(int CourseId) : IRequest<Course?>;
}
