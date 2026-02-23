using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCategoriesHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.GetCategories();
        }
    }
}
