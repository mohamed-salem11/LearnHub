using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class GetLessonsByUserHandler : IRequestHandler<GetLessonsByUserQuery, List<Lesson>>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetLessonsByUserHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<List<Lesson>> Handle(GetLessonsByUserQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.LessonsByUserAsync(request.UserId);
        }
    }
}
