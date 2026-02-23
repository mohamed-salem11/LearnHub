using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public class ApproveInstructorHandler : IRequestHandler<ApproveInstructorCommand, Unit>
    {
        private readonly IAdminRepository _adminRepository;

        public ApproveInstructorHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(ApproveInstructorCommand request, CancellationToken cancellationToken)
        {
            await _adminRepository.ApproveInstructorAsync(request.UserId);
            return Unit.Value;
        }
    }
}
