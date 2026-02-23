using LearnHub.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Commands.Enrollments
{
    public class BuyCourseHandler : IRequestHandler<BuyCourseCommand, Unit>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public BuyCourseHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Unit> Handle(BuyCourseCommand request, CancellationToken cancellationToken)
        {
            await _enrollmentRepository.BuyCourse(request.CourseId, request.UserId);
            return Unit.Value;
        }
    }
}
