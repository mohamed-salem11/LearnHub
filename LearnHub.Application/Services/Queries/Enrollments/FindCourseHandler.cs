using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Enrollments
{
    public class FindCourseHandler : IRequestHandler<FindCourseQuery, Course?>
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public FindCourseHandler(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public async Task<Course?> Handle(FindCourseQuery request, CancellationToken cancellationToken)
        {
            return await _enrollmentRepository.FindCourse(request.CourseId);
        }
    }
}
