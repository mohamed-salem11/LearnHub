using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Courses
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, (bool Success, string? Error, Course? Data)>
    {
        private readonly ICourseRepository _courseRepository;

        public CreateCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<(bool Success, string? Error, Course? Data)> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            if (request.ImageFile == null || request.ImageFile.Length == 0)
                return (false, "Please upload an image file.", null);

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return (false, "Only JPG, JPEG, PNG files are allowed.", null);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            request.Course.ApplicationUserId = request.UserId;
            request.Course.Status = CourseStatus.Draft;
            request.Course.TotalRating = 0;
            request.Course.CoverImageUrl = $"/uploads/{fileName}";

            var addedCourse = await _courseRepository.Add(request.Course);
            return (true, null, addedCourse);
        }
    }
}
