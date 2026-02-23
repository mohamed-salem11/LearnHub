using LearnHub.Domain.Entities;
using MediatR;
using System.Collections.Generic;

namespace LearnHub.Application.Services.Queries.Courses
{
    public record SearchCoursesQuery(string Query) : IRequest<List<Course>>;
}
