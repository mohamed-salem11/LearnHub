using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public record ApproveInstructorCommand(string UserId) : IRequest<Unit>;
}
