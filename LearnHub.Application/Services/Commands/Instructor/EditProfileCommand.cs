using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Instructor
{
    public record EditProfileCommand(string UserId, ApplicationUser ApplicationUser, IFormFile? PhotoFile)
        : IRequest<Unit>;
}
