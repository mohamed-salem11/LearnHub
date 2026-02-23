using LearnHub.Domain.Entities;
using MediatR;

namespace LearnHub.Application.Services.Queries.Instructor
{
    public record GetInstructorPageQuery(string UserId) : IRequest<ApplicationUser>;
}
