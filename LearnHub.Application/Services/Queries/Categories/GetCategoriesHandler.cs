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
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<Category>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            return await _categoryRepository.GetCategories();
        }
    }

}
