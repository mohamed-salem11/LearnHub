using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Admin
{
    public class GetInstructorRequestsHandler : IRequestHandler<GetInstructorRequestsQuery, List<ApplicationUser>>
    {
        private readonly IAdminRepository _adminRepository;

        public GetInstructorRequestsHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<ApplicationUser>> Handle(GetInstructorRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _adminRepository.GetInstructorRequestsAsync();
        }
    }
}
