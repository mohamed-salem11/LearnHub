using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class GetCourseWithLessonsHandler : IRequestHandler<GetCourseWithLessonsQuery, Course?>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetCourseWithLessonsHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<Course?> Handle(GetCourseWithLessonsQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.GetCourseWithLessonsAsync(request.CourseId);
        }
    }
}
