using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Courses
{
    public class RequestReviewHandler : IRequestHandler<RequestReviewCommand, bool>
    {
        private readonly ICourseRepository _courseRepository;

        public RequestReviewHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<bool> Handle(RequestReviewCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.Find(request.CourseId);
            if (course == null || course.ApplicationUserId != request.UserId)
                return false;

            course.Status = CourseStatus.Pending;
            await _courseRepository.Update(course);
            return true;
        }
    }
}
