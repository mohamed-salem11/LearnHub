using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Lessons
{
    public class UpdateLessonHandler : IRequestHandler<UpdateLessonCommand, (bool Success, string? Error, Lesson? Data)>
    {
        private readonly ILessonRepository _lessonRepository;

        public UpdateLessonHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<(bool Success, string? Error, Lesson? Data)> Handle(UpdateLessonCommand request, CancellationToken cancellationToken)
        {
            var existingLesson = await _lessonRepository.GetByIdAsync(request.Lesson.Id);
            if (existingLesson == null)
                return (false, "Lesson not found", null);

            existingLesson.Title = request.Lesson.Title;
            existingLesson.CourseId = request.Lesson.CourseId;

            if (request.VideoFile != null && request.VideoFile.Length > 0)
            {
                string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
                string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
                var fileExtension = Path.GetExtension(request.VideoFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(request.VideoFile.ContentType.ToLower()))
                    return (false, "Only video files are allowed.", null);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.VideoFile.CopyToAsync(stream);
                }

                existingLesson.VideoUrl = $"/uploads/videos/{fileName}";
            }

            var updatedLesson = await _lessonRepository.UpdateAsync(existingLesson);
            return (true, null, updatedLesson);
        }
    }
}
