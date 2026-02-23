using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public class RejectCourseHandler : IRequestHandler<RejectCourseCommand, Unit>
    {
        private readonly IAdminRepository _adminRepository;

        public RejectCourseHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(RejectCourseCommand request, CancellationToken cancellationToken)
        {
            await _adminRepository.RejectCourseAsync(request.CourseId);
            return Unit.Value;
        }
    }
}
