using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services
{
    public class LessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task<(bool Success, string? Error, Lesson? Data, string? FilePath)> CreateLessonAsync(Lesson lesson, IFormFile videoFile)
        {
            if (videoFile == null || videoFile.Length == 0)
                return (false, "Please upload a video file.", null, null);

            string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
            string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
            var fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(videoFile.ContentType.ToLower()))
                return (false, "Only video files (MP4, AVI, MOV, MKV, WEBM) are allowed.", null, null);

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            
            lesson.VideoUrl = $"/uploads/videos/{fileName}";
            var addedLesson = await _lessonRepository.AddAsync(lesson);

            return (true, null, addedLesson, filePath);
        }

        public async Task<(bool Success, string? Error, Lesson? Data)> UpdateLessonAsync(Lesson lesson, IFormFile? videoFile)
        {
            var existingLesson = await _lessonRepository.GetByIdAsync(lesson.Id);
            if (existingLesson == null)
                return (false, "Lesson not found", null);

            
            existingLesson.Title = lesson.Title;
            existingLesson.CourseId = lesson.CourseId;

        
            if (videoFile != null && videoFile.Length > 0)
            {
                string[] allowedExtensions = { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
                string[] allowedMimeTypes = { "video/mp4", "video/x-msvideo", "video/quicktime", "video/x-matroska", "video/webm" };
                var fileExtension = Path.GetExtension(videoFile.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension) || !allowedMimeTypes.Contains(videoFile.ContentType.ToLower()))
                    return (false, "Only video files are allowed.", null);

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "videos");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await videoFile.CopyToAsync(stream);
                }

                existingLesson.VideoUrl = $"/uploads/videos/{fileName}";
            }
          

            var updatedLesson = await _lessonRepository.UpdateAsync(existingLesson);
            return (true, null, updatedLesson);
        }

        public async Task<List<Course>> GetUserCoursesAsync(string userId)
        {
           
            return await _lessonRepository.GetUserCoursesAsync(userId);
        }
        public async Task<Course?> GetCourseWithLessonsAsync(int courseId)
        {
            return await _lessonRepository.GetCourseWithLessonsAsync(courseId);
        }
        public async Task<bool> CheckEnrollmentAsync(int courseId, string userId)
        {
            return await _lessonRepository.CheckEnrollmentAsync(courseId, userId);
        }

        public Task<Lesson?> GetByIdAsync(int id) => _lessonRepository.GetByIdAsync(id);
        public Task<List<Lesson>> LessonsByCourseAsync(int id) => _lessonRepository.LessonsByCourseAsync(id);
        public Task<List<Lesson>> LessonsByUserAsync(string userId) => _lessonRepository.LessonsByUserAsync(userId);
        public Task<Lesson> UpdateAsync(Lesson lesson) => _lessonRepository.UpdateAsync(lesson);
        public Task<bool> DeleteAsync(int id) => _lessonRepository.DeleteAsync(id);
    }
}
