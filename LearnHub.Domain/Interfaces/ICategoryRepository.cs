using LearnHub.Domain.Entities;

namespace LearnHub.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories();
        Task<Category> Add(Category category);
        Task<Category> Update(Category category);
        Task<Category> Delete(int id);
        Task<Category> Find(int id);
    }
}