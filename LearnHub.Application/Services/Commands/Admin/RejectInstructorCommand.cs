using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public record RejectInstructorCommand(string UserId) : IRequest<Unit>;
}
