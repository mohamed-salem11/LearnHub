using LearnHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnHub.Domain.Interfaces
{
    public interface IInstructorRepository
    {
        Task<ApplicationUser> GetInstructorPage(string id);
        Task EditProfile(ApplicationUser user);
    }
}
