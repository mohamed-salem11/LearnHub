using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public record GetBookedCoursesQuery(string UserId) : IRequest<List<Enrollment>>;
}
