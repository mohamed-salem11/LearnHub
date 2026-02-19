using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services
{
    public class InstructorRequestService
    {
        private readonly IInstructorRequestRepository _instructorRequestRepository;

        public InstructorRequestService(IInstructorRequestRepository instructorRequestRepository)
        {
            _instructorRequestRepository = instructorRequestRepository;
        }

        public async Task<(bool Success, string? Error)> SubmitRequestAsync(ApplicationUser user, IFormFile? photoFile)
        {
        
            if (photoFile != null && photoFile.Length > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return (false, "Only JPG and PNG images are allowed.");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photoFile.CopyToAsync(stream);
                }

                user.Photo = "/uploads/" + uniqueFileName;
            }

          
            await _instructorRequestRepository.SubmitRequest(user);
            return (true, null);
        }
    }
}
