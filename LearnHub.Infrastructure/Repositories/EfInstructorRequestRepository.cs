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
    public class EfInstructorRequestRepository : IInstructorRequestRepository
    {
        public ApplicationDbContext _applicationDbContext;
        public EfInstructorRequestRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

        }

        public async Task SubmitRequest(ApplicationUser applicationUser)
        {
              _applicationDbContext.ApplicationUsers.Update(applicationUser);
            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
