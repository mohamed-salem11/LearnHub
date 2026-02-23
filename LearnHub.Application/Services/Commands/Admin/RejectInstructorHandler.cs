using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Commands.Admin
{
    public class RejectInstructorHandler : IRequestHandler<RejectInstructorCommand, Unit>
    {
        private readonly IAdminRepository _adminRepository;

        public RejectInstructorHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Unit> Handle(RejectInstructorCommand request, CancellationToken cancellationToken)
        {
            await _adminRepository.RejectInstructorAsync(request.UserId);
            return Unit.Value;
        }
    }
}
