using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    public class RemoveRatingHandler : IRequestHandler<RemoveRatingCommand, Unit>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public RemoveRatingHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Unit> Handle(RemoveRatingCommand request, CancellationToken cancellationToken)
        {
            await _enrollmentRepository.RemoveRating(request.CourseId, request.UserId);
            return Unit.Value;
        }
    }
}
