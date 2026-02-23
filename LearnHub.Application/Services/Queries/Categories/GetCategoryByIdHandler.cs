using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services.Queries.Categories
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
        {
            return await _categoryRepository.Find(request.Id);
        }
    }
}
