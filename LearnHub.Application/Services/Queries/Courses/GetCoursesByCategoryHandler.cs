using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public class GetCoursesByCategoryHandler : IRequestHandler<GetCoursesByCategoryQuery, List<Course>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCoursesByCategoryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<Course>> Handle(GetCoursesByCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.CoursesByCategory(request.CategoryId);
        }
    }
}
 