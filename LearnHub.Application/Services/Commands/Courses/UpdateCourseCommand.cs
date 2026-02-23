using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Courses
{
    public record UpdateCourseCommand(int Id, Course UpdatedCourse, IFormFile? CoverImageFile) : IRequest<Unit>;
}
