using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Lessons
{
    public class DeleteLessonHandler : IRequestHandler<DeleteLessonCommand, bool>
    {
        private readonly ILessonRepository _lessonRepository;

        public DeleteLessonHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<bool> Handle(DeleteLessonCommand request, CancellationToken cancellationToken)
        {
            return await _lessonRepository.DeleteAsync(request.Id);
        }
    }
}
