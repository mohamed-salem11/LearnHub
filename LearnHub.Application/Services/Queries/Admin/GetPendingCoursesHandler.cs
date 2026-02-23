using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Admin
{
    public class GetPendingCoursesHandler : IRequestHandler<GetPendingCoursesQuery, List<Course>>
    {
        private readonly IAdminRepository _adminRepository;

        public GetPendingCoursesHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<List<Course>> Handle(GetPendingCoursesQuery request, CancellationToken cancellationToken)
        {
            return await _adminRepository.GetPendingCoursesAsync();
        }
    }
}
