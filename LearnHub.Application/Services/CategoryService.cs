using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _categoryRepository.GetCategories();
        }

        public async Task<Category?> Find(int id)
        {
            return await _categoryRepository.Find(id);
        }

        public async Task<(bool Success, string? Error, Category? Data)> Add(Category category, IFormFile imageFile)
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

            category.CoverImageUrl = $"/uploads/{fileName}";
            var addedCategory = await _categoryRepository.Add(category);
            return (true, null, addedCategory);
        }

        public async Task Update(Category category)
        {
            await _categoryRepository.Update(category);
        }

        public async Task Delete(int id)
        {
            await _categoryRepository.Delete(id);
        }
    }
}