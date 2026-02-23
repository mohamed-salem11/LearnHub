using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Courses
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Course?>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseByIdHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _courseRepository.Find(request.Id);
        }
    }
}
