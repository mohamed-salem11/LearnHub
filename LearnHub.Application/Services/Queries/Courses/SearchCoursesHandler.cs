using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public class SearchCoursesHandler : IRequestHandler<SearchCoursesQuery, List<Course>>
    {
        private readonly ICourseRepository _courseRepository;

        public SearchCoursesHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<Course>> Handle(SearchCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.Search(request.Query);
        }
    }
}
