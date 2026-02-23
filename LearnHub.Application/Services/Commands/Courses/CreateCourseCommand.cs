using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Courses
{
    public record CreateCourseCommand(Course Course, IFormFile ImageFile, string UserId) : IRequest<(bool Success, string? Error, Course? Data)>;
}
