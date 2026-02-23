using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.InstructorRequest
{
    public class SubmitInstructorRequestHandler : IRequestHandler<SubmitInstructorRequestCommand, (bool Success, string? Error)>
    {
        private readonly IInstructorRequestRepository _instructorRequestRepository;

        public SubmitInstructorRequestHandler(IInstructorRequestRepository instructorRequestRepository)
        {
            _instructorRequestRepository = instructorRequestRepository;
        }

        public async Task<(bool Success, string? Error)> Handle(SubmitInstructorRequestCommand request, CancellationToken cancellationToken)
        {
            var user = request.User;

            if (request.PhotoFile != null && request.PhotoFile.Length > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(request.PhotoFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return (false, "Only JPG and PNG images are allowed.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.PhotoFile.CopyToAsync(stream);
                }

                user.Photo = "/uploads/" + uniqueFileName;
            }

            await _instructorRequestRepository.SubmitRequest(user);
            return (true, null);
        }
    }
}
