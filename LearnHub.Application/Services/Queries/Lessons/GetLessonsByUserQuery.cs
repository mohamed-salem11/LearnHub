using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public record GetLessonsByUserQuery(string UserId) : IRequest<List<Lesson>>;
}
