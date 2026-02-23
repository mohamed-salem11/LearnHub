using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class GetLessonsByCourseHandler : IRequestHandler<GetLessonsByCourseQuery, List<Lesson>>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetLessonsByCourseHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<List<Lesson>> Handle(GetLessonsByCourseQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.LessonsByCourseAsync(request.CourseId);
        }
    }
}
