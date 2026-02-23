using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Commands.Categories
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, (bool Success, string? Error, Category? Data)>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<(bool Success, string? Error, Category? Data)> Handle(CreateCategoryCommand request, CancellationToken ct)
        {
            var imageFile = request.ImageFile;
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

            request.Category.CoverImageUrl = $"/uploads/{fileName}";
            var addedCategory = await _categoryRepository.Add(request.Category);
            return (true, null, addedCategory);
        }
    }
}