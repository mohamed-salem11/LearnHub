using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Lessons
{
    public class CreateLessonHandler : IRequestHandler<CreateLessonCommand, (bool Success, string? Error, Lesson? Data, string? FilePath)>
    {
        private readonly ILessonRepository _lessonRepository;

        public CreateLessonHandler(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<(bool Success, string? Error, Lesson? Data, string? FilePath)> Handle(CreateLessonCommand request, CancellationToken cancellationToken)
        {
            if (request.VideoFile == null || request.VideoFile.Length == 0)
                return (false, "Please upload a video file.", null, null);

            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
            string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
            var fileExtension = Path.GetExtension(request.VideoFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(request.VideoFile.ContentType.ToLower()))
                return (false, "Only video files are allowed.", null, null);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            request.Lesson.VideoUrl = $"/uploads/videos/{fileName}";
            var addedLesson = await _lessonRepository.AddAsync(request.Lesson);

            return (true, null, addedLesson, filePath);
        }
    }
}
