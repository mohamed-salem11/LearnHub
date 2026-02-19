using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Application.Services
{
    public class InstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }
        public async Task<ApplicationUser> GetInstructorPage(string id)
        {
           return await _instructorRepository.GetInstructorPage(id);
        }

        public async Task EditProfile(string userId,ApplicationUser applicationUser, IFormFile? newImage)
        {
            var user = await _instructorRepository.GetInstructorPage(userId);
            if (user == null || !user.IsInstructor) return;
           
            user.Bio = applicationUser.Bio;
            user.Specialization = applicationUser.Specialization;
            user.IsInstructorRequestPending = true;

            if (newImage != null && newImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(newImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(fileStream);
                }

                user.Photo = "/uploads/" + uniqueFileName;
            }
              await _instructorRepository.EditProfile(user);
        }



    }
}




