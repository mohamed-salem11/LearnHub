using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    public class AddRatingHandler : IRequestHandler<AddRatingCommand, Unit>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public AddRatingHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Unit> Handle(AddRatingCommand request, CancellationToken cancellationToken)
        {
            await _enrollmentRepository.AddRating(request.CourseId, request.UserId, request.Rating);
            return Unit.Value;
        }
    }
}
