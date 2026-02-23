using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class GetUserCoursesHandler : IRequestHandler<GetUserCoursesQuery, List<Course>>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetUserCoursesHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<List<Course>> Handle(GetUserCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.GetUserCoursesAsync(request.UserId);
        }
    }
}
