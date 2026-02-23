using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;

namespace LearnHub.Application.Services.Queries.Instructor
{
    public class GetInstructorPageHandler : IRequestHandler<GetInstructorPageQuery, ApplicationUser>
    {
        private readonly IInstructorRepository _instructorRepository;

        public GetInstructorPageHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<ApplicationUser> Handle(GetInstructorPageQuery request, CancellationToken cancellationToken)
        {
            return await _instructorRepository.GetInstructorPage(request.UserId);
        }
    }
}
