using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class GetLessonByIdHandler : IRequestHandler<GetLessonByIdQuery, Lesson?>
    {
        private readonly ILessonRepository _lessonRepository;

        public GetLessonByIdHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<Lesson?> Handle(GetLessonByIdQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.GetByIdAsync(request.Id);
        }
    }
}
