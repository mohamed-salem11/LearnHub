using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public record GetLessonsByCourseQuery(int CourseId) : IRequest<List<Lesson>>;
}
