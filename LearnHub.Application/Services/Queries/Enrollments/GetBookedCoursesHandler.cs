using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public class GetBookedCoursesHandler : IRequestHandler<GetBookedCoursesQuery, List<Enrollment>>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public GetBookedCoursesHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<List<Enrollment>> Handle(GetBookedCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository.GetBookedCourses(request.UserId);
        }
    }
}
