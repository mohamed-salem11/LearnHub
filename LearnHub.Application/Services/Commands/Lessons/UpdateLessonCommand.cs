using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Lessons
{
    public record UpdateLessonCommand(Lesson Lesson, IFormFile? VideoFile) : IRequest<(bool Success, string? Error, Lesson? Data)>;
}
