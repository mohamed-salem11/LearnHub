using LearnHub.Domain.Entities;
using LearnHub.Domain.Interfaces;
using LearnHub.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Infrastructure.Repositories
{
    public class EfInstructorRepository : IInstructorRepository
    {
        public ApplicationDbContext _applicationDbContext;
        public EfInstructorRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public  async Task EditProfile(ApplicationUser user)
        {
            _applicationDbContext.Update(user);
            await _applicationDbContext.SaveChangesAsync();
 
        }

        public async Task<ApplicationUser> GetInstructorPage(string id)
        {
             return await _applicationDbContext.Users
               .Include(u => u.Courses)
               .FirstOrDefaultAsync(u => u.Id == id && u.IsInstructor);
        }
    }
}
