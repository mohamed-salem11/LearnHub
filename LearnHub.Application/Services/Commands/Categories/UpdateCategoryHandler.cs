using LearnHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Commands.Categories
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Unit>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var existingCategory = await _categoryRepository.Find(request.Category.Id);
            if (existingCategory == null) return Unit.Value;

            existingCategory.Name = request.Category.Name;

            if (request.ImageFile != null && request.ImageFile.Length > 0)
            { 
                existingCategory.CoverImageUrl = "/uploads/new-file-name.jpg";
            }

            await _categoryRepository.Update(existingCategory);
            return Unit.Value; 
        }
    }

}
