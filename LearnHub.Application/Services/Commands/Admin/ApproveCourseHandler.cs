using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public class ApproveCourseHandler : IRequestHandler<ApproveCourseCommand, Unit>
    {
        private readonly IAdminRepository _adminRepository;

        public ApproveCourseHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(ApproveCourseCommand request, CancellationToken cancellationToken)
        {
            await _adminRepository.ApproveCourseAsync(request.CourseId);
            return Unit.Value;
        }
    }
}
