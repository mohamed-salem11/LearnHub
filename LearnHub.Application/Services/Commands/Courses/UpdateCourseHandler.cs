using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace LearnHub.Application.Services.Commands.Courses
{
    public class UpdateCourseHandler : IRequestHandler<UpdateCourseCommand, Unit>
    {
        private readonly ICourseRepository _courseRepository;

        public UpdateCourseHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Unit> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var existingCourse = await _courseRepository.Find(request.Id);
            if (existingCourse == null) return Unit.Value;

            existingCourse.Title = request.UpdatedCourse.Title;
            existingCourse.Description = request.UpdatedCourse.Description;
            existingCourse.Price = request.UpdatedCourse.Price;
            existingCourse.CategoryId = request.UpdatedCourse.CategoryId;

            if (request.CoverImageFile != null && request.CoverImageFile.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CoverImageFile.FileName)}";
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                if (!Directory.Exists(uploadPath)) Directory.CreateDirectory(uploadPath);

                var filePath = Path.Combine(uploadPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CoverImageFile.CopyToAsync(stream);
                }
                existingCourse.CoverImageUrl = $"/uploads/{fileName}";
            }

            await _courseRepository.Update(existingCourse);
            return Unit.Value;
        }
    }
}
