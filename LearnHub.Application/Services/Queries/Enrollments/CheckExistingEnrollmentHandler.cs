using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public class CheckExistingEnrollmentHandler : IRequestHandler<CheckExistingEnrollmentQuery, Enrollment?>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public CheckExistingEnrollmentHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Enrollment?> Handle(CheckExistingEnrollmentQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository.CheckExistingEnrollment(request.CourseId, request.UserId);
        }
    }
}
