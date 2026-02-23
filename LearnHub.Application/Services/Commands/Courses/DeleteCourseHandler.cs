using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Courses
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Unit>
    {
        private readonly ICourseRepository _courseRepository;

        public DeleteCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            await _courseRepository.Delete(request.Id);
            return Unit.Value;
        }
    }
}
