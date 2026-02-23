using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Admin
{
    public record GetPendingCoursesQuery() : IRequest<List<Course>>;
}
