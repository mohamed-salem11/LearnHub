using LearnHub.Domain.Interfaces;
using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Instructor
{
    public class EditProfileHandler : IRequestHandler<EditProfileCommand, Unit>
    {
        private readonly IInstructorRepository _instructorRepository;

        public EditProfileHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<Unit> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _instructorRepository.GetInstructorPage(request.UserId);
            if (user == null || !user.IsInstructor) return Unit.Value;

            user.Bio = request.ApplicationUser.Bio;
            user.Specialization = request.ApplicationUser.Specialization;
            user.IsInstructorRequestPending = true;

            if (request.PhotoFile != null && request.PhotoFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.PhotoFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.PhotoFile.CopyToAsync(fileStream);
                }

                user.Photo = "/uploads/" + uniqueFileName;
            }

            await _instructorRepository.EditProfile(user);
            return Unit.Value;
        }
    }
}
