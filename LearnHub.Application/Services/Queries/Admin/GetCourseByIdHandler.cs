using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Admin
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Course?>
    {
        private readonly IAdminRepository _adminRepository;

        public GetCourseByIdHandler(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Course?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            return await _adminRepository.FindCourseByIdAsync(request.CourseId);
        }
    }
}
