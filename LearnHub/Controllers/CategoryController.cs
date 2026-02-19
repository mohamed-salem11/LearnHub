using LearnHub.Application.Services;
using LearnHub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearnHub.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategories();  
            return View(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Category category, IFormFile imageFile)
        {
            var result = await _categoryService.Add(category, imageFile);
            if (!result.Success)
            {
                ModelState.AddModelError("imageFile", result.Error!);
                return View(category);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryService.Find(id);  
            if (category == null) return NotFound();
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Category category, IFormFile imageFile)
        {
            var existingCategory = await _categoryService.Find(category.Id);
            if (existingCategory == null) return NotFound();

            existingCategory.Name = category.Name;
 
            if (imageFile != null && imageFile.Length > 0)
           {  
                existingCategory.CoverImageUrl = "/uploads/new-file-name.jpg";  
            }

            await _categoryService.Update(existingCategory);  
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.Find(id);
            if (category == null)
                return NotFound();
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.Delete(id); 
            return RedirectToAction(nameof(Index));
        }
    }
}