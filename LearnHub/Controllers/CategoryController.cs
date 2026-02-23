using LearnHub.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnHub.Application.Services.Commands.Categories;
using LearnHub.Application.Services.Queries.Categories;

namespace LearnHub.Controllers
{
    public class CategoryController : Controller
    { 
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return View(categories);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return View();  
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Category category, IFormFile imageFile)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(category, imageFile));
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
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category == null) return NotFound();
            return View(category); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(Category category, IFormFile? imageFile)
        {
            await _mediator.Send(new UpdateCategoryCommand(category, imageFile));
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category == null) return NotFound();
            return View(category); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
