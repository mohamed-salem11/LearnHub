using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Lessons
{
    public class CheckEnrollmentHandler : IRequestHandler<CheckEnrollmentQuery, bool>
    {
        private readonly ILessonRepository _lessonRepository;

        public CheckEnrollmentHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<bool> Handle(CheckEnrollmentQuery request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.CheckEnrollmentAsync(request.CourseId, request.UserId);
        }
    }
}
