using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.InstructorRequest
{
    public record SubmitInstructorRequestCommand(ApplicationUser User, IFormFile? PhotoFile)
        : IRequest<(bool Success, string? Error)>;
}
