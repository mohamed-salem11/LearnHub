using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public record GetLessonByIdQuery(int Id) : IRequest<Lesson?>;
}
