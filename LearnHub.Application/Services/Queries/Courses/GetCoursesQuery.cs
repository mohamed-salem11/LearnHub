using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Courses
{
    public record GetCoursesQuery() : IRequest<List<Course>>;
}
