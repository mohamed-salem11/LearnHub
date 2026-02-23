using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, List<Course>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCoursesHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<Course>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.GetCourses();
        }
    }
}
