using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace LearnHub.Application.Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
 
        public async Task<List<Course>> GetCourses()
        {
            return await _courseRepository.GetCourses();
        }
         
        public async Task<List<Course>> CoursesByCategory(int id)
        {
            return await _courseRepository.CoursesByCategory(id);
        }
         
        public async Task<List<Course>> Search(string query)
        {
            return await _courseRepository.Search(query);
        }

        public async Task<(bool Success, string? Error, Course? Data)> Add(Course course, IFormFile imageFile, string userId)
        {
            if (imageFile == null || imageFile.Length == 0)
                return (false, "Please upload an image file.", null);

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return (false, "Only JPG, JPEG, PNG files are allowed.", null);

            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            course.ApplicationUserId = userId;
            course.Status = CourseStatus.Draft;
            course.TotalRating = 0;
            course.CoverImageUrl = $"/uploads/{fileName}";
            var addedCourse = await _courseRepository.Add(course);
            return (true, null, addedCourse);
        }

        public async Task Update(int id, Course updatedData, IFormFile? newImage)
        {
            var existingCourse = await _courseRepository.Find(id);

            if (existingCourse == null) return;
            existingCourse.Title = updatedData.Title;
            existingCourse.Description = updatedData.Description;
            existingCourse.Price = updatedData.Price;
            existingCourse.CategoryId = updatedData.CategoryId;

     
            if (newImage != null && newImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(newImage.FileName)}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }
                existingCourse.CoverImageUrl = $"/uploads/{fileName}";
            }
            await _courseRepository.Update(existingCourse);
        }
  
         
        public async Task Delete (int id)
        {
            await _courseRepository.Delete(id);
        }

    

            public async Task<bool> RequestReview(int courseId, string userId)
            {
          
                var course = await _courseRepository.Find(courseId);
                if (course == null || course.ApplicationUserId != userId)
                    return false;

                course.Status = CourseStatus.Pending;
                await _courseRepository.Update(course);
                return true;
            }
        

        public async Task<Course?> Find(int id)
        {
            var course = await _courseRepository.Find(id);
            return course;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _courseRepository.GetCategories();
        }

    }
}
